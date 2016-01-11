using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Business.Models;
using Famoser.BeerCompanion.Business.Repository.Interfaces;
using Famoser.BeerCompanion.Common.Framework.Logging;
using Famoser.BeerCompanion.Data.Entities;
using Famoser.BeerCompanion.Data.Entities.Communication;
using Famoser.BeerCompanion.Data.Services;
using Newtonsoft.Json;
using IStorageService = Famoser.BeerCompanion.Business.Services.IStorageService;

namespace Famoser.BeerCompanion.Business.Repository
{
    public class SettingsRepository : ISettingsRepository
    {
        private IStorageService _storageService;
        private IDataService _dataService;
        public SettingsRepository(IStorageService storageService, IDataService dataService)
        {
            _storageService = storageService;
            _dataService = dataService;
        }

        private UserInformations _userInformations;
        public async Task<UserInformations> GetUserInformations()
        {
            if (_userInformations == null)
            {
                try
                {

                    var res = await _storageService.GetUserInformations();
                    if (!string.IsNullOrEmpty(res))
                    { 
                        var ui = JsonConvert.DeserializeObject<UserInformations>(res);
                        var beers = await _storageService.GetUserBeers();
                        if (!string.IsNullOrEmpty(beers))
                            ui.Beers = JsonConvert.DeserializeObject<ObservableCollection<Beer>>(beers);
                        _userInformations = ui;
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Instance.LogException(ex, this);
                }
            }
            if (_userInformations == null)
                _userInformations = await CreateNewUser();

            return _userInformations;
        }

        private async Task<UserInformations> CreateNewUser()
        {
            var newui = new UserInformations()
            {
                Guid = Guid.NewGuid(),
                FirstTime = true
            };
            var str = JsonConvert.SerializeObject(newui);
            if (!await _storageService.SetUserInformations(str))
                LogHelper.Instance.Log(LogLevel.FatalError, this, "Cannot save User Informations");
            return newui;
        }

        public async Task<bool> SaveUserInformations(UserInformations userInformations)
        {

            try
            {
                _userInformations = userInformations;
                var settings = JsonConvert.SerializeObject(userInformations);
                var beers = JsonConvert.SerializeObject(userInformations.Beers);
                return await _storageService.SetUserInformations(settings) && await _storageService.SetUserBeers(beers);
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex, this);
            }
            return false;
        }

        public async Task<bool> SyncUserInformations(UserInformations userInformations)
        {

            try
            {
                var drinkerInfo = new DrinkerRequest()
                {
                    Guid = userInformations.Guid,
                    Name = userInformations.Name,
                    Color = userInformations.Color
                };
                return await _dataService.UpdateDrinker(drinkerInfo);
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex, this);
            }
            return false;
        }

        public UserInformations GetSampleUserInformations()
        {
            return new UserInformations()
            {
                Beers = new ObservableCollection<Beer>()
                {
                    new Beer() {Guid = Guid.Empty,Posted = true,DrinkTime = DateTime.Now},
                    new Beer() {Guid = Guid.Empty,Posted = true,DrinkTime = DateTime.Now},
                    new Beer() {Guid = Guid.Empty,Posted = true,DrinkTime = DateTime.Now},
                    new Beer() {Guid = Guid.Empty,Posted = true,DrinkTime = DateTime.Now},
                    new Beer() {Guid = Guid.Empty,Posted = true,DrinkTime = DateTime.Now},
                },
                Guid = Guid.NewGuid(),
                Name = "Florian"
            };
        }
    }
}
