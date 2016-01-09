using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Famoser.BeerCompanion.Common.Framework.Logging;
using Famoser.BeerCompanion.Common.Services;

namespace Famoser.BeerCompanion.Presentation.WinUniversal.Platform
{
    public class StorageService : IStorageService
    {
        private async Task<string> ReadCache(string filename)
        {
            try
            {
                StorageFile localFile = await ApplicationData.Current.LocalFolder.GetFileAsync(filename);
                if (localFile != null)
                {
                    return await FileIO.ReadTextAsync(localFile);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Log(LogLevel.Error, this, "ReadCache failed", ex);
            }
            return null;
        }

        private async Task<string> ReadSettings(string filename)
        {
            try
            {
                StorageFile localFile = await ApplicationData.Current.RoamingFolder.GetFileAsync(filename);
                if (localFile != null)
                {
                    return await FileIO.ReadTextAsync(localFile);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Log(LogLevel.Error, this, "ReadSettings failed", ex);
            }
            return null;
        }
        private async Task<bool> SaveToCache(string filename, string content)
        {
            try
            {
                StorageFile localFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(filename,CreationCollisionOption.ReplaceExisting);
                if (localFile != null)
                {
                    await FileIO.WriteTextAsync(localFile, content);
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Log(LogLevel.Error, this, "SaveToCache failed", ex);
            }
            return false;
        }

        private async Task<bool> SaveToSettings(string filename, string content)
        {
            try
            {
                StorageFile localFile = await ApplicationData.Current.RoamingFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                if (localFile != null)
                {
                    await FileIO.WriteTextAsync(localFile, content);
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Log(LogLevel.Error, this, "SaveToSettings failed", ex);
            }
            return false;
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
    }
}
