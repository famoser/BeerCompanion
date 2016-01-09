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
        private const string ApiUrl = "http://api.beercompanion.famoser.ch/";
        public Task<string> GetDrinkerCycle(Guid ownId)
        {
            return DownloadString(new Uri(ApiUrl + "cycles/" + ownId));
        }

        public Task<bool> PostDrinkerCycle(string json)
        {
            return Post(new Uri(ApiUrl + "cycles/act"), json);
        }

        public Task<string> GetBeers(Guid ownId)
        {
            return DownloadString(new Uri(ApiUrl + "beers/" + ownId));
        }

        public Task<bool> PostBeers(string json)
        {
            return Post(new Uri(ApiUrl + "beers/add"), json);
        }

        public Task<bool> DeleteBeers(string json)
        {
            return Delete(new Uri(ApiUrl + "beers/delete"), json);
        }

        public Task<bool> UpdateDrinker(string json)
        {
            return Post(new Uri(ApiUrl + "drinkers/update"), json);
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

        private async Task<bool> Delete(Uri url, string content)
        {
            var res = await Post(url, content);
            if (res)
                return true;
            return false;
        }
    }
}
