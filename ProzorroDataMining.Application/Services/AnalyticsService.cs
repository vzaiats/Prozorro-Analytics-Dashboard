using ProzorroDataMining.Application.Interfaces.Services;
using ProzorroDataMining.Domain.Interfaces.Repositories;
using ProzorroDataMining.Domain.Results;

namespace ProzorroDataMining.Application.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IAnalyticsRepository _repository;

        #region Ctor

        public AnalyticsService(IAnalyticsRepository repository)
        {
            _repository = repository;
        }

        #endregion

        #region Methods

        // Returns total savings
        public Task<AnalyticsResult> GetTotalSavingsAsync()
        {
            return _repository.GetTotalSavingsAsync();
        }

        // Returns top procurers
        public Task<IEnumerable<AnalyticsResult>> GetTopProcurersAsync()
        {
            return _repository.GetTopProcurersAsync();
        }

        // Returns top suppliers
        public Task<IEnumerable<AnalyticsResult>> GetTopSuppliersAsync()
        {
            return _repository.GetTopSuppliersAsync();
        }

        #endregion
    }
}
