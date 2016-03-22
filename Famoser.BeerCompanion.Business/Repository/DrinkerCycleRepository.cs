using System;
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
using Famoser.BeerCompanion.Data.Enums;
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
        private ISettingsRepository _settingsRepository;

        public DrinkerCycleRepository(IDataService dataService, IStorageService storageService, ISettingsRepository settingsRepository)
        {
            _dataService = dataService;
            _storageService = storageService;
            _settingsRepository = settingsRepository;
        }

        public async Task<ObservableCollection<DrinkerCycle>> GetSavedCycles()
        {
            try
            {
                var str = await _storageService.GetCachedData();

                if (!string.IsNullOrEmpty(str))
                {
                    var usrInfo = await SimpleIoc.Default.GetInstance<ISettingsRepository>().GetUserInformations();
                    if (usrInfo != null)
                    {
                        var obj = JsonConvert.DeserializeObject<CycleSaveModel>(str);
                        return ConstructBusinessModel(obj.Drinkers.ToList(), obj.DrinkerCycles.ToList(), usrInfo);
                    }
                }
                else
                {
                    var usrInfo = await SimpleIoc.Default.GetInstance<ISettingsRepository>().GetUserInformations();
                    if (usrInfo != null)
                    {
                        return await AktualizeCycles();
                    }
                }
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
                    Name = "Hogwarts",
                    AuthBeerDrinkers = new ObservableCollection<Person>()
                    {
                        new Drinker() {Name = "Markus", LastBeer = DateTime.Now,TotalBeers = 12, Color = "673AB7"},
                        new Drinker() {Name = "Felix", LastBeer = DateTime.Now,TotalBeers = 42, Color = "03A9F4"},
                        new Drinker() {Name = "Bastian", LastBeer = DateTime.Now,TotalBeers = 11, Color = "F44336"},
                        new Drinker() {Name = "Alex", LastBeer = DateTime.Now,TotalBeers = 9, Color = "4CAF50"}
                    },
                    NonAuthBeerDrinkers = new ObservableCollection<Person>()
                    {
                        new Drinker() {Name = "Dr gruusigi neui", LastBeer = DateTime.Now,TotalBeers = 2, Color = "FFC107"},
                    },
                    IsAuthenticated = true
                },
                new DrinkerCycle()
                {
                    Name = "RC Baden Baden",
                    AuthBeerDrinkers = new ObservableCollection<Person>()
                    {
                        new Drinker() {Name = "Bastian", LastBeer = DateTime.Now,TotalBeers = 31, Color = "32425a"},
                        new Drinker() {Name = "Selina", LastBeer = DateTime.Now,TotalBeers = 2, Color = "3f425a"},
                        new Drinker() {Name = "Gertrude", LastBeer = DateTime.Now,TotalBeers = 8, Color = "3f425a"},
                        new Drinker() {Name = "Mr. D", LastBeer = DateTime.Now,TotalBeers = 51, Color = "3f425a"}
                    },
                    NonAuthBeerDrinkers = new ObservableCollection<Person>()
                    {
                        new Drinker() {Name = "Dr gruusigi neui 1", LastBeer = DateTime.Now,TotalBeers = 2, Color = "3f425a"},
                        new Drinker() {Name = "Dr gruusigi neui 2", LastBeer = DateTime.Now,TotalBeers = 2, Color = "3f425a"},
                    },
                    IsAuthenticated = true
                },
                new DrinkerCycle()
                {
                    Name = "Illuminati",
                    IsAuthenticated = false
                }
            };

            //close circle
            foreach (var drinkerCycle in obs)
            {
                foreach (var beerDrinker in drinkerCycle.AuthBeerDrinkers)
                {
                    if (beerDrinker.AuthDrinkerCycles == null)
                        beerDrinker.AuthDrinkerCycles = new ObservableCollection<DrinkerCycle>();
                    if (beerDrinker.NonAuthDrinkerCycles == null)
                        beerDrinker.NonAuthDrinkerCycles = new ObservableCollection<DrinkerCycle>();

                    beerDrinker.AuthDrinkerCycles.Add(drinkerCycle);
                }

                foreach (var nonAuthBeerDrinker in drinkerCycle.NonAuthBeerDrinkers)
                {
                    if (nonAuthBeerDrinker.AuthDrinkerCycles == null)
                        nonAuthBeerDrinker.AuthDrinkerCycles = new ObservableCollection<DrinkerCycle>();
                    if (nonAuthBeerDrinker.NonAuthDrinkerCycles == null)
                        nonAuthBeerDrinker.NonAuthDrinkerCycles = new ObservableCollection<DrinkerCycle>();

                    nonAuthBeerDrinker.NonAuthDrinkerCycles.Add(drinkerCycle);
                }
            }

            //reference some in both groups
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
                    
                    return ConstructBusinessModel(drinkers, drinkerCycles, usrInfo);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex, this);
            }
            return null;
        }

        private ObservableCollection<DrinkerCycle> ConstructBusinessModel(List<Drinker> drinkers,
            List<DrinkerCycle> drinkerCycles, UserInformations infos)
        {
            foreach (var drinker in drinkers)
            {
                foreach (var authDrinkerCycle in drinker.AuthDrinkerCycleGuids)
                {
                    var authCircle = drinkerCycles.FirstOrDefault(ds => ds.Guid == authDrinkerCycle);
					if (authCircle != null)
                    	authCircle.AuthBeerDrinkers.Add(drinker);
                }
                foreach (var nonAuthDrinkerCycle in drinker.NonAuthDrinkerCycleGuids)
                {
					var nonAuthCircle = drinkerCycles.FirstOrDefault(ds => ds.Guid == nonAuthDrinkerCycle);
					if (nonAuthCircle != null)
                    	nonAuthCircle.NonAuthBeerDrinkers.Add(drinker);
                }
            }
            foreach (var drinkerCycle in drinkerCycles)
            {
                if (drinkerCycle.IsAuthenticated)
                    drinkerCycle.AuthBeerDrinkers.Add(infos);
                else
                    drinkerCycle.NonAuthBeerDrinkers.Add(infos);
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
                        {
                            var drinker = authBeerDrinker as Drinker;
                            if (drinker != null)
                                drinkers.Add(drinker);
                        }
                    }
                    foreach (var nonAuthBeerDrinker in drinkerCycle.NonAuthBeerDrinkers)
                    {
                        if (!drinkers.Contains(nonAuthBeerDrinker))
                        {
                            var drinker = nonAuthBeerDrinker as Drinker;
                            if (drinker != null)
                                drinkers.Add(drinker);
                        }
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
            return MakeRequest(name, Guid.Empty, PossibleActions.Exists);
        }

        public async Task<bool> AddSelf(string name)
        {
            var ui = await _settingsRepository.GetUserInformations();
            return await MakeRequest(name, ui.Guid, PossibleActions.Add);
        }

        public async Task<bool> RemoveSelf(string name)
        {
            var ui = await _settingsRepository.GetUserInformations();
            return await MakeRequest(name, ui.Guid, PossibleActions.Remove);
        }

        public async Task<bool> AuthenticateUser(string name, Guid authGuid)
        {
            var ui = await _settingsRepository.GetUserInformations();
            return await MakeRequest(name, ui.Guid, PossibleActions.Autheticate, authGuid);
        }

        public async Task<bool> DeAuthenticateUser(string name, Guid deAuthGuid)
        {
            var ui = await _settingsRepository.GetUserInformations();
            return await MakeRequest(name, ui.Guid, PossibleActions.Deautheticate, deAuthGuid);
        }

        public async Task<bool> RemoveUser(string name, Guid deAuthGuid)
        {
            var ui = await _settingsRepository.GetUserInformations();
            return await MakeRequest(name, ui.Guid, PossibleActions.RemoveForeign, deAuthGuid);
        }

        private async Task<bool> MakeRequest(string name, Guid userGuid, PossibleActions actionName, Guid? authGuid = null)
        {
            try
            {
                var res = await _dataService.PostDrinkerCycle(RequestConverter.Instance.ConvertToDrinkerCycleRequest(userGuid,
                            actionName, name, authGuid));
                return res.Response && res.IsSuccessfull;
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex, this);
            }
            return false;
        }
    }
}
