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
using Famoser.BeerCompanion.Data.Entities.Communication.Generic;
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
            if (resp.IsSuccessfull)
            {
                try
                {
                    return JsonConvert.DeserializeObject<DrinkerCycleResponse>(resp.Response);
                }
                catch (Exception ex)
                {
                    LogHelper.Instance.Log(LogLevel.ApiError, this, "GetBeers failed with response: " + resp, ex);
                    return new DrinkerCycleResponse()
                    {
                        ErrorMessage = "Unserialisation failed for Content " + resp.Response
                    };
                }
            }
            return new DrinkerCycleResponse()
            {
                ErrorMessage = resp.ErrorMessage
            };
        }

        public Task<BooleanResponse> PostDrinkerCycle(DrinkerCycleRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            return Post(new Uri(ApiUrl + "cycles/act"), json);
        }

        public async Task<BeerResponse> GetBeers(Guid ownId)
        {
            var resp = await DownloadString(new Uri(ApiUrl + "beers/" + ownId));
            if (resp.IsSuccessfull)
            {
                try
                {
                    return JsonConvert.DeserializeObject<BeerResponse>(resp.Response);
                }
                catch (Exception ex)
                {
                    LogHelper.Instance.Log(LogLevel.ApiError, this, "GetBeers failed with response: " + resp, ex);
                    return new BeerResponse()
                    {
                        ErrorMessage = "Unserialisation failed for Content " + resp.Response
                    };
                }
            }
            return new BeerResponse()
            {
                ErrorMessage = resp.ErrorMessage
            };
        }

        public Task<BooleanResponse> PostBeer(BeerRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            return Post(new Uri(ApiUrl + "beers/act"), json);
        }

        public Task<BooleanResponse> PostDrinker(DrinkerRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            return Post(new Uri(ApiUrl + "drinkers/act"), json);
        }

        public async Task<DrinkerResponse> GetDrinker(Guid ownId)
        {
            var resp = await DownloadString(new Uri(ApiUrl + "drinkers/" + ownId));
            if (resp.IsSuccessfull)
            {
                try
                {
                    return JsonConvert.DeserializeObject<DrinkerResponse>(resp.Response);
                }
                catch (Exception ex)
                {
                    LogHelper.Instance.Log(LogLevel.ApiError, this, "GetDrinker failed with response: " + resp.Response, ex);
                    return new DrinkerResponse()
                    {
                        ErrorMessage = "Unserialisation failed for Content " + resp.Response
                    };
                }
            }
            return new DrinkerResponse()
            {
                ErrorMessage = resp.ErrorMessage
            };
        }

        private async Task<StringReponse> DownloadString(Uri url)
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
                    var resp = await client.GetAsync(url);
                    var res = new StringReponse()
                    {
                        Response = await resp.Content.ReadAsStringAsync()
                    };
                    if (resp.IsSuccessStatusCode)
                        return res;
                    res.ErrorMessage = "Request not successfull: Status Code " + resp.StatusCode + " returned";
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Log(LogLevel.Error, this, "DownloadStringAsync failed for url " + url, ex);
                return new StringReponse()
                {
                    ErrorMessage = "Request failed for url " + url
                };
            }
        }

        private async Task<BooleanResponse> Post(Uri url, string content)
        {
            BooleanResponse resp = null;
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
                        resp = new BooleanResponse() { Response = true };
                    else
                    {
                        resp = new BooleanResponse() { ErrorMessage = respo };
                        LogHelper.Instance.Log(LogLevel.ApiError, this,
                            "Post failed for url " + url + " with json " + content + " Reponse recieved: " + respo);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Log(LogLevel.Error, this, "Post failed for url " + url, ex);
                resp = new BooleanResponse() { ErrorMessage = "Post failed for url " + url };
            }
            return resp;
        }
    }
}
