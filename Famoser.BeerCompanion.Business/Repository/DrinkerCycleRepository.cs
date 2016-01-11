﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Business.Converter;
using Famoser.BeerCompanion.Business.Models;
using Famoser.BeerCompanion.Business.Models.Save;
using Famoser.BeerCompanion.Business.Repository.Interfaces;
using Famoser.BeerCompanion.Common.Framework.Logging;
using Famoser.BeerCompanion.Data;
using Famoser.BeerCompanion.Data.Entities;
using Famoser.BeerCompanion.Data.Entities.Communication;
using Famoser.BeerCompanion.Data.Services;
using GalaSoft.MvvmLight.Ioc;
using Newtonsoft.Json;
using IStorageService = Famoser.BeerCompanion.Business.Services.IStorageService;

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
                    AuthBeerDrinkers = new ObservableCollection<Drinker>()
                    {
                        new Drinker() {Name = "Markus", LastBeer = DateTime.Now,TotalBeers = 12},
                        new Drinker() {Name = "Felix", LastBeer = DateTime.Now,TotalBeers = 42},
                        new Drinker() {Name = "Bastian", LastBeer = DateTime.Now,TotalBeers = 11},
                        new Drinker() {Name = "Alex", LastBeer = DateTime.Now,TotalBeers = 9}
                    },
                    NonAuthBeerDrinkers = new ObservableCollection<Drinker>()
                    {
                        new Drinker() {Name = "Dr gruusigi neui", LastBeer = DateTime.Now,TotalBeers = 2},
                    },
                },
                new DrinkerCycle()
                {
                    AuthBeerDrinkers = new ObservableCollection<Drinker>()
                    {
                        new Drinker() {Name = "Bastian", LastBeer = DateTime.Now,TotalBeers = 31},
                        new Drinker() {Name = "Selina", LastBeer = DateTime.Now,TotalBeers = 2},
                        new Drinker() {Name = "Gertrude", LastBeer = DateTime.Now,TotalBeers = 8},
                        new Drinker() {Name = "Mr. D", LastBeer = DateTime.Now,TotalBeers = 51}
                    },
                    NonAuthBeerDrinkers = new ObservableCollection<Drinker>()
                    {
                        new Drinker() {Name = "Dr gruusigi neui 1", LastBeer = DateTime.Now,TotalBeers = 2},
                        new Drinker() {Name = "Dr gruusigi neui 2", LastBeer = DateTime.Now,TotalBeers = 2},
                    },
                },
            };

            //close circle
            foreach (var drinkerCycle in obs)
            {
                foreach (var beerDrinker in drinkerCycle.AuthBeerDrinkers)
                {
                    beerDrinker.DrinkerCycles = new ObservableCollection<DrinkerCycle>()
                    {
                        drinkerCycle
                    };
                }
            }

            //reference some both
            obs[0].AuthBeerDrinkers.Add(obs[1].AuthBeerDrinkers[0]);
            obs[0].AuthBeerDrinkers.Add(obs[1].AuthBeerDrinkers[2]);
            return obs;
        }

        public async Task<ObservableCollection<DrinkerCycle>> AktualizeCycles()
        {
            try
            {
                var usrInfo = await SimpleIoc.Default.GetInstance<ISettingsRepository>().GetUserInformations();
                if (usrInfo != null)
                {
                    var drinkerResponse = await DataService.Instance.GetDrinkerCycle(usrInfo.Guid);
                    var drinkerCycles = ResponseConverter.Instance.Convert(drinkerResponse.DrinkerCycles);
                    var drinkers = ResponseConverter.Instance.Convert(drinkerResponse.Drinkers);

                    return ConstructBusinessModel(drinkers, drinkerCycles);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex, this);
            }
            return null;
        }

        private ObservableCollection<DrinkerCycle> ConstructBusinessModel(List<Drinker> drinkers,
            List<DrinkerCycle> drinkerCycles)
        {
            foreach (var drinker in drinkers)
            {
                foreach (var authDrinkerCycle in drinker.AuthDrinkerCycles)
                {
                    var authCircle = drinkerCycles.FirstOrDefault(ds => ds.Guid == authDrinkerCycle);
                    authCircle?.AuthBeerDrinkers.Add(drinker);
                }
                foreach (var nonAuthDrinkerCycle in drinker.NonAuthDrinkerCycles)
                {
                    var nonAuthCircle = drinkerCycles.FirstOrDefault(ds => ds.Guid == nonAuthDrinkerCycle);
                    nonAuthCircle?.NonAuthBeerDrinkers.Add(drinker);
                }
            }
            return new ObservableCollection<DrinkerCycle>(drinkerCycles);
        }

        public async Task<bool> SaveCycles(ObservableCollection<DrinkerCycle> drinkerCycles)
        {
            try
            {
                var drinkers = new ObservableCollection<Drinker>();
                foreach (var drinkerCycle in drinkerCycles)
                {
                    foreach (var authBeerDrinker in drinkerCycle.AuthBeerDrinkers)
                    {
                        if (!drinkers.Contains(authBeerDrinker))
                            drinkers.Add(authBeerDrinker);
                    }
                    foreach (var nonAuthBeerDrinker in drinkerCycle.NonAuthBeerDrinkers)
                    {
                        if (!drinkers.Contains(nonAuthBeerDrinker))
                            drinkers.Add(nonAuthBeerDrinker);
                    }
                }
                var sm = new CycleSaveModel()
                {
                    DrinkerCycles = drinkerCycles,
                    Drinkers = drinkers
                };
                var str = JsonConvert.SerializeObject(sm);
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
                return await _dataService.PostDrinkerCycle(RequestConverter.Instance.ConvertToDrinkerCycleRequest(name,userGuid,actionName));
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex, this);
            }
            return false;
        }
    }
}
