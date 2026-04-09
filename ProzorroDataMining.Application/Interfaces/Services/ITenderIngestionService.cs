namespace ProzorroDataMining.Application.Interfaces.Services
{
    public interface ITenderIngestionService
    {
        Task FetchAndStoreRecentTendersAsync();
    }
}
