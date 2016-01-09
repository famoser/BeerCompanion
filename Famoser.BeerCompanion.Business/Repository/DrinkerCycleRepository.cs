using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Business.Models;
using Famoser.BeerCompanion.Business.Repository.Interfaces;
using Famoser.BeerCompanion.Common.Framework.Logging;
using Famoser.BeerCompanion.Common.Services;
using Famoser.BeerCompanion.Data;
using Famoser.BeerCompanion.Data.Entities;
using GalaSoft.MvvmLight.Ioc;
using Newtonsoft.Json;

namespace Famoser.BeerCompanion.Business.Repository
{
    public class DrinkerCycleRepository : IDrinkerCycleRepository
    {
        private IStorageService _storageService;
        private IDataService _dataService;

        public DrinkerCycleRepository(IDataService dataService, IStorageService storageService)
        {
            _dataService = dataService;
            _storageService = storageService;
        }

        public async Task<ObservableCollection<DrinkerCycle>> GetSavedCycles()
        {
            try
            {
                var str = await _storageService.GetCachedData();
                if (!string.IsNullOrEmpty(str))
                    return JsonConvert.DeserializeObject<ObservableCollection<DrinkerCycle>>(str);
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex, this);
            }
            return new ObservableCollection<DrinkerCycle>();
        }

        public ObservableCollection<DrinkerCycle> GetSampleCycles()
        {
            var obs = new ObservableCollection<DrinkerCycle>()
            {
                new DrinkerCycle()
                {
                    BeerDrinkers = new ObservableCollection<Drinker>()
                    {
                        new Drinker() {Name = "Markus", LastBeer = DateTime.Now,TotalBeers = 12},
                        new Drinker() {Name = "Felix", LastBeer = DateTime.Now,TotalBeers = 42},
                        new Drinker() {Name = "Bastian", LastBeer = DateTime.Now,TotalBeers = 11},
                        new Drinker() {Name = "Alex", LastBeer = DateTime.Now,TotalBeers = 9}
                    }
                },
                new DrinkerCycle()
                {
                    BeerDrinkers = new ObservableCollection<Drinker>()
                    {
                        new Drinker() {Name = "Bastian", LastBeer = DateTime.Now,TotalBeers = 31},
                        new Drinker() {Name = "Selina", LastBeer = DateTime.Now,TotalBeers = 2},
                        new Drinker() {Name = "Gertrude", LastBeer = DateTime.Now,TotalBeers = 8},
                        new Drinker() {Name = "Mr. D", LastBeer = DateTime.Now,TotalBeers = 51}
                    }
                },
            };

            //close circle
            foreach (var drinkerCycle in obs)
            {
                foreach (var beerDrinker in drinkerCycle.BeerDrinkers)
                {
                    beerDrinker.DrinkerCycles = new ObservableCollection<DrinkerCycle>()
                    {
                        drinkerCycle
                    };
                }
            }

            //reference some both
            obs[0].BeerDrinkers.Add(obs[1].BeerDrinkers[0]);
            obs[0].BeerDrinkers.Add(obs[1].BeerDrinkers[2]);
            return obs;
        }

        public async Task<ObservableCollection<DrinkerCycle>> AktualizeCycles()
        {
            try
            {
                var usrInfo = await SimpleIoc.Default.GetInstance<ISettingsRepository>().GetUserInformations();
                if (usrInfo != null)
                {
                    var str = await DataService.Instance.GetDrinkerCycle(usrInfo.Guid);
                    return JsonConvert.DeserializeObject<ObservableCollection<DrinkerCycle>>(str);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex, this);
            }
            return null;
        }

        public async Task<bool> SaveCycles(ObservableCollection<DrinkerCycle> drinkerCycles)
        {
            try
            {
                var str = JsonConvert.SerializeObject(drinkerCycles);
                return await _storageService.SetCachedData(str);
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex, this);
            }
            return false;
        }


        public Task<bool> DoesExists(string name)
        {
            return MakeRequest(name, Guid.Empty, "exist");
        }

        public Task<bool> AddSelf(string name, Guid onwGuid)
        {
            return MakeRequest(name, onwGuid, "add");
        }

        public Task<bool> RemoveSelf(string name, Guid onwGuid)
        {
            return MakeRequest(name, onwGuid, "remove");
        }

        public Task<bool> AuthenticateUser(string name, Guid userGuid)
        {
            return MakeRequest(name, userGuid, "authenticate");
        }

        public Task<bool> DeAuthenticateUser(string name, Guid userGuid)
        {
            return MakeRequest(name, userGuid, "deauthenticate");
        }

        private async Task<bool> MakeRequest(string name, Guid userGuid, string actionName)
        {
            try
            {
                var comm = new DrinkerCycleCommunication() {Action = actionName, Guid = userGuid, Name = name};
                var json =JsonConvert.SerializeObject(comm);
                return await _dataService.PostDrinkerCycle(json);
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex, this);
            }
            return false;
        }
    }
}
