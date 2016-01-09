using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Business.Converter;
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
    public class BeerRepository : IBeerRepository
    {
        private IStorageService _storageService;
        private IDataService _dataService;

        public BeerRepository(IStorageService storageService, IDataService dataService)
        {
            _storageService = storageService;
            _dataService = dataService;
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
            return new ObservableCollection<DrinkerCycle>();
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

        public async Task<ObservableCollection<Beer>> SyncBeers()
        {
            try
            {
                var usrInfo = await SimpleIoc.Default.GetInstance<ISettingsRepository>().GetUserInformations();
                if (usrInfo != null)
                {
                    //remove deleted & not posted
                    var beers = usrInfo.Beers;
                    var remove = usrInfo.Beers.Where(b => b.DeletePending && !b.Posted);
                    foreach (var beer in remove)
                    {
                        beers.Remove(beer);
                    }

                    //remove deleted
                    var deleted = beers.Where(b => b.DeletePending).ToList();
                    if (deleted.Any())
                    {
                        var obj = EntityConverter.Instance.ConvertToBeerCollectionEntity(usrInfo.Guid, deleted);
                        var json = JsonConvert.SerializeObject(obj);
                        if (await _dataService.DeleteBeers(json))
                        {
                            foreach (var beer in deleted)
                            {
                                beers.Remove(beer);
                            }
                        }
                    }

                    //add new
                    var add = beers.Where(b => !b.Posted).ToList();
                    if (add.Any())
                    {
                        var obj = EntityConverter.Instance.ConvertToBeerCollectionEntity(usrInfo.Guid, add);
                        var json = JsonConvert.SerializeObject(obj);
                        if (await _dataService.PostBeers(json))
                        {
                            foreach (var beer in beers)
                            {
                                beer.Posted = true;
                            }
                        }
                    }

                    return beers;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex, this);
            }
            return false;
        }

        public async Task<bool> RemoveLastBeers(int count)
        {
            try
            {
                var usrInfo = await SimpleIoc.Default.GetInstance<ISettingsRepository>().GetUserInformations();
                if (usrInfo != null)
                {
                    var entity = new BeerRemoveEntity()
                    {
                        Guid = usrInfo.Guid,
                        Count = count
                    };
                    var json = JsonConvert.SerializeObject(entity);
                    return await _dataService.RemoveLastBeers(json);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex, this);
            }
            return false;
        }
    }
}
