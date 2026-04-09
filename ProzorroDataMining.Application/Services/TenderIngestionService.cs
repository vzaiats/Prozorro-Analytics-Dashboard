using Microsoft.Extensions.Logging;
using ProzorroDataMining.Application.Interfaces.Services;
using ProzorroDataMining.Domain.Entities;
using ProzorroDataMining.Domain.Interfaces.Repositories;
using ProzorroDataMining.Domain.Models;
using System.Text.Json;

namespace ProzorroDataMining.Application.Services
{
    public class TenderIngestionService : ITenderIngestionService
    {
        private readonly HttpClient _httpClient;
        private readonly ITenderRepository _repository;
        private readonly ILogger<TenderIngestionService> _logger;

        #region Ctor

        public TenderIngestionService(HttpClient httpClient, ITenderRepository repository, ILogger<TenderIngestionService> logger)
        {
            _httpClient = httpClient;
            _repository = repository;
            _logger = logger;
        }

        #endregion

        #region Methods

        // Get recent tenders from Prozorro API and store them in the database
        public async Task FetchAndStoreRecentTendersAsync()
        {
            try
            {
                _logger.LogInformation("Starting ETL process: clearing old tenders...");
                await _repository.ClearAllTendersAsync();

                _logger.LogInformation("Waiting for database cleanup...");
                // Delay to ensure database cleanup is completed before fetching new tenders
                await Task.Delay(TimeSpan.FromMilliseconds(500));

                _logger.LogInformation("Fetching recent tenders...");

                var response = await _httpClient.GetAsync("tenders?descending=1");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);

                var tenders = doc.RootElement.GetProperty("data").EnumerateArray().ToList();
                _logger.LogInformation("Fetched {Count} tenders from Prozorro API", tenders.Count);

                var semaphore = new SemaphoreSlim(5);

                await Parallel.ForEachAsync(tenders, async (tenderElement, token) =>
                {
                    var tenderId = tenderElement.GetProperty("id").GetString();
                    if (string.IsNullOrEmpty(tenderId)) return;

                    await semaphore.WaitAsync(token);
                    try
                    {
                        var detailResponse = await _httpClient.GetAsync($"tenders/{tenderId}", token);
                        detailResponse.EnsureSuccessStatusCode();

                        var detailJson = await detailResponse.Content.ReadAsStringAsync(token);
                        using var detailDoc = JsonDocument.Parse(detailJson);

                        var data = detailDoc.RootElement.GetProperty("data");

                        var tender = new Tender
                        {
                            TenderId = data.GetProperty("id").GetString() ?? tenderId,
                            Status = data.TryGetProperty("status", out var statusProp) ? statusProp.GetString() ?? "unknown" : "unknown",
                            Budget = data.TryGetProperty("value", out var valueProp) && valueProp.TryGetProperty("amount", out var amountProp)
                                ? amountProp.GetDecimal()
                                : 0,
                            ProcuringEntity = data.TryGetProperty("procuringEntity", out var peProp) && peProp.TryGetProperty("name", out var nameProp)
                                ? nameProp.GetString() ?? "unknown"
                                : "unknown",
                            TenderDate = data.TryGetProperty("date", out var dateProp) && DateTime.TryParse(dateProp.GetString(), out var parsedDate)
                                ? parsedDate
                                : DateTime.UtcNow,
                            ImportedAt = DateTime.UtcNow
                        };

                        // Extract CPV code
                        if (data.TryGetProperty("items", out var items))
                        {
                            foreach (var item in items.EnumerateArray())
                            {
                                if (item.TryGetProperty("classification", out var classification) &&
                                    classification.TryGetProperty("id", out var cpvProp))
                                {
                                    tender.CpvCode = cpvProp.GetString();
                                    break;
                                }
                            }
                        }

                        // Extract contracts
                        if (data.TryGetProperty("contracts", out var contracts))
                        {
                            foreach (var contractElement in contracts.EnumerateArray())
                            {
                                if (contractElement.TryGetProperty("value", out var contractValue))
                                {
                                    tender.Contracts.Add(new Contract
                                    {
                                        TenderId = tender.TenderId,
                                        Amount = contractValue.TryGetProperty("amount", out var amountVal)
                                            ? amountVal.GetDecimal()
                                            : 0
                                    });
                                }
                            }
                        }

                        // Extract suppliers
                        if (data.TryGetProperty("awards", out var awards))
                        {
                            foreach (var award in awards.EnumerateArray())
                            {
                                if (award.TryGetProperty("suppliers", out var suppliers))
                                {
                                    foreach (var supplier in suppliers.EnumerateArray())
                                    {
                                        tender.Suppliers.Add(new Supplier
                                        {
                                            TenderId = tender.TenderId,
                                            Name = supplier.TryGetProperty("name", out var supName)
                                                ? supName.GetString() ?? "unknown"
                                                : "unknown"
                                        });
                                    }
                                }
                            }
                        }

                        //Filter before inserting (filtering tenders by CPV, status, and date range)
                        //if (tender.CpvCode == Constants.Constants.TargetCpvCode
                        //    && tender.Status == Constants.Constants.TargetStatus
                        //    && tender.TenderDate >= DateTime.UtcNow.Subtract(Constants.Constants.TargetDateRange))
                        //{
                            await _repository.InsertTenderAsync(tender);
                            _logger.LogInformation("Tender {TenderId} imported successfully", tender.TenderId);
                        //}
                        //else
                        //{
                        //    _logger.LogInformation("Tender {TenderId} skipped", tender.TenderId);
                        //}
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing tender {TenderId}", tenderId);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });

                _logger.LogInformation("ETL process completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in FetchAndStoreRecentTendersAsync");
            }
        }

        #endregion
    }
}
