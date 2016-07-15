using System;
using System.Threading.Tasks;
using Windows.Storage;
using Famoser.BeerCompanion.Business.Enums;
using Famoser.BeerCompanion.Business.Services;
using Famoser.FrameworkEssentials.Logging;
using Famoser.FrameworkEssentials.Services.Base;

namespace Famoser.BeerCompanion.Presentation.WindowsUniversal.Platform
{
    public class StorageService : BaseService, IStorageService
    {
        private Task<string> ReadCache(string filename)
        {
            return Execute(async () =>
            {
                StorageFile localFile = await ApplicationData.Current.LocalFolder.GetFileAsync(filename);
                return await FileIO.ReadTextAsync(localFile);
            });
        }

        private Task<string> ReadSettings(string filename)
        {
            return Execute(async () =>
            {
                StorageFile localFile = await ApplicationData.Current.RoamingFolder.GetFileAsync(filename);
                return await FileIO.ReadTextAsync(localFile);
            });
        }


        private Task<string> ReadAsset(string filename)
        {
            return Execute(async () =>
            {
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Configuration/" + filename));
                return await FileIO.ReadTextAsync(file);
            });
        }

        private Task<bool> SaveToCache(string filename, string content)
        {
            return Execute(async () =>
            {
                StorageFile localFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                if (localFile != null)
                {
                    await FileIO.WriteTextAsync(localFile, content);
                    return true;
                }
                return false;
            });
        }

        private Task<bool> SaveToSettings(string filename, string content)
        {
            return Execute(async () =>
            {
                StorageFile localFile = await ApplicationData.Current.RoamingFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                if (localFile != null)
                {
                    await FileIO.WriteTextAsync(localFile, content);
                    return true;
                }
                return false;
            });
        }

        public Task<string> GetCachedData()
        {
            return ReadCache("data.json");
        }

        public Task<string> GetUserInformations()
        {
            return ReadSettings("user.json");
        }

        public Task<string> GetUserBeers()
        {
            return ReadCache("beers.json");
        }

        public Task<string> GetAssetFile(AssetFileKeys fileKey)
        {
            if (fileKey == AssetFileKeys.ColorsJson)
                return ReadAsset("ColorsJson.json");
            return null;
        }

        public Task<bool> SetCachedData(string data)
        {
            return SaveToCache("data.json", data);
        }

        public Task<bool> SetUserInformations(string info)
        {
            return SaveToSettings("user.json", info);
        }

        public Task<bool> SetUserBeers(string info)
        {
            return SaveToCache("beers.json", info);
        }

        public async Task<bool> ResetApplication()
        {
            try
            {
                var allfiles = await ApplicationData.Current.LocalFolder.GetFilesAsync();
                foreach (var storageFile in allfiles)
                {
                    await storageFile.DeleteAsync();
                }

                allfiles = await ApplicationData.Current.RoamingFolder.GetFilesAsync();
                foreach (var storageFile in allfiles)
                {
                    await storageFile.DeleteAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex, this);
            }
            return false;
        }
    }
}
