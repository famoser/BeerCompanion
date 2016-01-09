using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Business.Models;
using Famoser.BeerCompanion.Business.Repository.Interfaces;
using Famoser.BeerCompanion.Common.Framework.Logging;
using Famoser.BeerCompanion.Common.Services;
using Famoser.BeerCompanion.Data.Entities;
using Newtonsoft.Json;

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

        public async Task<UserInformations> GetUserInformations()
        {
            try
            {
                var res = await _storageService.GetUserInformations();
                if (string.IsNullOrEmpty(res))
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
                var ui = JsonConvert.DeserializeObject<UserInformations>(res);
                var beers = await _storageService.GetUserBeers();
                if (!string.IsNullOrEmpty(beers))
                    ui.Beers = JsonConvert.DeserializeObject<ObservableCollection<Beer>>(beers);
                return ui;
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex, this);
            }
            return null;
        }

        public async Task<bool> SaveUserInformations(UserInformations userInformations, bool updateOnline)
        {

            try
            {
                var settings = JsonConvert.SerializeObject(userInformations);
                var beers = JsonConvert.SerializeObject(userInformations.Beers);
                if (updateOnline)
                {
                    var drinkerInfo = new DrinkerCommunication()
                    {
                        Guid = userInformations.Guid,
                        Name = userInformations.Name
                    };
                    var json = JsonConvert.SerializeObject(drinkerInfo);
                    if (!await _dataService.UpdateDrinker(json))
                        return false;
                }
                return await _storageService.SetUserInformations(settings) && await _storageService.SetUserBeers(beers);
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
