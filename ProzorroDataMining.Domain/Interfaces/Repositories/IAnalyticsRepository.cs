using ProzorroDataMining.Domain.Results;

namespace ProzorroDataMining.Domain.Interfaces.Repositories
{
    public interface IAnalyticsRepository
    {
        Task<AnalyticsResult> GetTotalSavingsAsync();
        Task<IEnumerable<AnalyticsResult>> GetTopProcurersAsync();
        Task<IEnumerable<AnalyticsResult>> GetTopSuppliersAsync();
    }
}
