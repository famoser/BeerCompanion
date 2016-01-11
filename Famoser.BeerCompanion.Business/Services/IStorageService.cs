using System.Threading.Tasks;
using Famoser.BeerCompanion.Business.Enums;

namespace Famoser.BeerCompanion.Business.Services
{
    public interface IStorageService
    {
        /// <summary>
        /// Get cached Data (saved on every Device)
        /// </summary>
        /// <returns></returns>
        Task<string> GetCachedData();
        /// <summary>
        /// Get User informations, the same for all devices of a single User
        /// </summary>
        /// <returns></returns>
        Task<string> GetUserInformations();
        /// <summary>
        /// Get Beers added on this Device
        /// </summary>
        /// <returns></returns>
        Task<string> GetUserBeers();
        
        /// <summary>
        /// Get File from Solution Folder
        /// </summary>
        /// <param name="fileKey"></param>
        /// <returns></returns>
        Task<string> GetAssetFile(AssetFileKeys fileKey);

        Task<bool> SetCachedData(string data);
        Task<bool> SetUserInformations(string info);
        Task<bool> SetUserBeers(string info);
    }
}
