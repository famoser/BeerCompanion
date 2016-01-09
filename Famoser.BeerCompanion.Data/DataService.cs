using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Common.Framework.Logging;
using Famoser.BeerCompanion.Common.Framework.Singleton;
using Famoser.BeerCompanion.Common.Services;
using Famoser.BeerCompanion.Data.Entities;

namespace Famoser.BeerCompanion.Data
{
    public class DataService : SingletonBase<DataService>, IDataService
    {
        private const string ApiUrl = "http://beercompanionserverapi.azurewebsites.net";
        public async Task<string> GetDrinkerCycle(Guid ownId)
        {
            try
            {
                return await DownloadString(new Uri(ApiUrl + "cycles/" + ownId));
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Log(LogLevel.Error, this, "GetDrinkerCycle failed for Owner " + ownId, ex);
            }
            return null;
        }

        public async Task<bool> PostBeers(string json)
        {
            try
            {
                return await Post(new Uri(ApiUrl + "beers/add"), json);
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Log(LogLevel.Error, this, "PostBeers failed", ex);
            }
            return false;
        }

        public async Task<bool> RemoveLastBeers(string json)
        {
            try
            {
                return await Post(new Uri(ApiUrl + "beers/remove"), json);
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Log(LogLevel.Error, this, "RemoveLastBeers failed", ex);
            }
            return false;
        }

        private async Task<string> DownloadString(Uri url)
        {
            try
            {
                using (var client = new HttpClient(
                    new HttpClientHandler
                    {
                        AutomaticDecompression = DecompressionMethods.GZip
                                                 | DecompressionMethods.Deflate
                    }))
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");

                    return await client.GetStringAsync(url);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Log(LogLevel.Error, this, "DownloadStringAsync failed for url " + url, ex);
            }
            return null;
        }

        private async Task<bool> Post(Uri url, string content)
        {
            try
            {
                using (var client = new HttpClient(
                    new HttpClientHandler
                    {
                        AutomaticDecompression = DecompressionMethods.GZip
                                                 | DecompressionMethods.Deflate
                    }))
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");

                    var res = await client.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));
                    return res.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Log(LogLevel.Error, this, "Post failed for url " + url, ex);
            }
            return false;
        }
    }
}
