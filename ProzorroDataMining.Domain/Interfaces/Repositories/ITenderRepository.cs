using ProzorroDataMining.Domain.Models;

namespace ProzorroDataMining.Domain.Interfaces.Repositories
{
    public interface ITenderRepository
    {
        Task InsertTenderAsync(Tender tender);

        Task<Tender?> GetTenderByIdAsync(string tenderId);

        Task ClearAllTendersAsync();
    }
}
