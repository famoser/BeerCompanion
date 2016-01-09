using System.Threading.Tasks;

namespace Famoser.BeerCompanion.Common.Services
{
    public interface IStorageService
    {
        Task<string> GetCachedData();
        Task<string> GetUserInformations();
        Task<string> GetUserBeers();

        Task<bool> SetCachedData(string data);
        Task<bool> SetUserInformations(string info);
        Task<bool> SetUserBeers(string info);
    }
}
