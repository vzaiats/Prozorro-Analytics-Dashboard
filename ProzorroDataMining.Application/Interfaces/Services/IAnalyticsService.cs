using ProzorroDataMining.Domain.Results;

namespace ProzorroDataMining.Application.Interfaces.Services
{
    public interface IAnalyticsService
    {
        Task<AnalyticsResult> GetTotalSavingsAsync();
        Task<IEnumerable<AnalyticsResult>> GetTopProcurersAsync();
        Task<IEnumerable<AnalyticsResult>> GetTopSuppliersAsync();
    }
}
