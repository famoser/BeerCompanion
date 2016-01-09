using System.Threading.Tasks;

namespace Famoser.BeerCompanion.Common.Services
{
    public interface IStorageService
    {
        Task<string> GetCachedData();
        Task<string> GetUserInformations();

        Task<bool> SetUserInformations(string info);
        Task<bool> SetCachedData(string data);
    }
}
