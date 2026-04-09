using Dapper;
using Microsoft.Extensions.Logging;
using ProzorroDataMining.Data.Constants;
using ProzorroDataMining.Domain.Interfaces.Repositories;
using ProzorroDataMining.Domain.Results;
using System.Data.Common;

namespace ProzorroDataMining.Data.Repository
{
    public class AnalyticsRepository : IAnalyticsRepository
    {
        private readonly DbConnection _connection;
        private readonly ILogger<AnalyticsRepository> _logger;

        #region Ctor

        public AnalyticsRepository(DbConnection connection, ILogger<AnalyticsRepository> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        #endregion

        #region Methods

        // Calculates total savings across all tenders
        public async Task<AnalyticsResult> GetTotalSavingsAsync()
        {
            try
            {
                var savings = await _connection.ExecuteScalarAsync<decimal>(SqlQueriesConstants.GetTotalSavings);
                return new AnalyticsResult { Savings = savings };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTotalSavingsAsync");
                return new AnalyticsResult { Savings = 0 };
            }
        }

        // Retrieves top procuring entities ranked by contract amount
        public async Task<IEnumerable<AnalyticsResult>> GetTopProcurersAsync()
        {
            try
            {
                return await _connection.QueryAsync<AnalyticsResult>(SqlQueriesConstants.GetTopProcurers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTopProcurersAsync");
                return Enumerable.Empty<AnalyticsResult>();
            }
        }

        // Retrieves top suppliers ranked by awarded contract amount
        public async Task<IEnumerable<AnalyticsResult>> GetTopSuppliersAsync()
        {
            try
            {
                return await _connection.QueryAsync<AnalyticsResult>(SqlQueriesConstants.GetTopSuppliers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTopSuppliersAsync");
                return Enumerable.Empty<AnalyticsResult>();
            }
        }

        #endregion
    }
}
