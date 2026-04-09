using Dapper;
using Microsoft.Extensions.Logging;
using ProzorroDataMining.Data.Constants;
using ProzorroDataMining.Domain.Entities;
using ProzorroDataMining.Domain.Interfaces.Repositories;
using ProzorroDataMining.Domain.Models;
using System.Data.Common;

namespace ProzorroDataMining.Data.Repository
{
    public class TenderRepository : ITenderRepository
    {
        private readonly DbConnection _connection;
        private readonly ILogger<TenderRepository> _logger;

        #region Ctor

        public TenderRepository(DbConnection connection, ILogger<TenderRepository> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        #endregion

        #region Methods

        // Inserts tender along with related contracts and suppliers
        public async Task InsertTenderAsync(Tender tender)
        {
            try
            {
                await _connection.ExecuteAsync(SqlQueriesConstants.InsertTender, tender);

                foreach (var contract in tender.Contracts)
                {
                    contract.ContractId = Guid.NewGuid();
                    contract.TenderId = tender.TenderId;
                    await _connection.ExecuteAsync(SqlQueriesConstants.InsertContract, contract);
                }

                foreach (var supplier in tender.Suppliers)
                {
                    supplier.SupplierId = Guid.NewGuid();
                    supplier.TenderId = tender.TenderId;
                    await _connection.ExecuteAsync(SqlQueriesConstants.InsertSupplier, supplier);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting tender {TenderId}", tender.TenderId);
            }
        }

        // Retrieves tender by ID including contracts and suppliers
        public async Task<Tender?> GetTenderByIdAsync(string tenderId)
        {
            try
            {
                var tender = await _connection.QueryFirstOrDefaultAsync<Tender>(
                    SqlQueriesConstants.GetTenderById,
                    new { TenderId = tenderId });

                if (tender == null) return null;

                tender.Contracts = (await _connection.QueryAsync<Contract>(
                    SqlQueriesConstants.GetContractsByTenderId,
                    new { TenderId = tenderId })).ToList();

                tender.Suppliers = (await _connection.QueryAsync<Supplier>(
                    SqlQueriesConstants.GetSuppliersByTenderId,
                    new { TenderId = tenderId })).ToList();

                return tender;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching tender {TenderId}", tenderId);
                return null;
            }
        }

        // Clears all tenders, contracts, and suppliers from database
        public async Task ClearAllTendersAsync()
        {
            try
            {
                var hasData = await _connection.ExecuteScalarAsync<bool>(SqlQueriesConstants.CheckIfAnyDataExists);

                if (!hasData)
                {
                    _logger.LogInformation("Database is already empty, nothing to delete.");
                    return;
                }

                await _connection.ExecuteAsync(SqlQueriesConstants.DeleteContracts);
                await _connection.ExecuteAsync(SqlQueriesConstants.DeleteSuppliers);
                await _connection.ExecuteAsync(SqlQueriesConstants.DeleteTenders);

                _logger.LogInformation("All tenders, contracts, and suppliers deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing tenders");
                throw;
            }
        }

        #endregion
    }
}
