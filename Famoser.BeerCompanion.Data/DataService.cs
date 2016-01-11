using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Common.Framework.Logging;
using Famoser.BeerCompanion.Common.Framework.Singleton;
using Famoser.BeerCompanion.Data.Entities;
using Famoser.BeerCompanion.Data.Entities.Communication;
using Famoser.BeerCompanion.Data.Services;
using Newtonsoft.Json;

namespace Famoser.BeerCompanion.Data
{
    public class DataService : SingletonBase<DataService>, IDataService
    {
        private const string ApiUrl = "http://api.beercompanion.famoser.ch/";
        public async Task<DrinkerCycleResponse> GetDrinkerCycle(Guid ownId)
        {
            var resp = await DownloadString(new Uri(ApiUrl + "cycles/" + ownId));
            try
            {
                return JsonConvert.DeserializeObject<DrinkerCycleResponse>(resp);
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Log(LogLevel.ApiError, this, "GetBeers failed with response: " + resp, ex);
            }
            return null;
        }

        public Task<bool> PostDrinkerCycle(DrinkerCycleRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            return Post(new Uri(ApiUrl + "cycles/act"), json);
        }

        public async Task<BeerResponse> GetBeers(Guid ownId)
        {
            var resp = await DownloadString(new Uri(ApiUrl + "beers/" + ownId));
            try
            {
                return JsonConvert.DeserializeObject<BeerResponse>(resp);
            }
            catch (Exception ex)
            { 
                LogHelper.Instance.Log(LogLevel.ApiError, this,"GetBeers failed with response: " + resp, ex);
            }
            return null;
        }

        public Task<bool> PostBeers(BeerRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            return Post(new Uri(ApiUrl + "beers/act"), json);
        }

        public Task<bool> UpdateDrinker(DrinkerRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            return Delete(new Uri(ApiUrl + "beers/delete"), json);
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
                    var respo = await res.Content.ReadAsStringAsync();
                    if (respo == "1")
                        return true;
                    LogHelper.Instance.Log(LogLevel.ApiError, this, "Post failed for url " + url + " with json " + content + " Reponse recieved: " + respo);
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
