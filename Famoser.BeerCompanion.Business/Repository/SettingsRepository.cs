using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Business.Models;
using Famoser.BeerCompanion.Business.Repository.Interfaces;
using Famoser.BeerCompanion.Common.Framework.Logging;
using Famoser.BeerCompanion.Common.Services;
using Newtonsoft.Json;

namespace Famoser.BeerCompanion.Business.Repository
{
    public class SettingsRepository : ISettingsRepository
    {
        private IStorageService _storageService;
        public SettingsRepository(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public async Task<UserInformations> GetUserInformations()
        {
            try
            {
                var res = await _storageService.GetUserInformations();
                if (string.IsNullOrEmpty(res))
                {
                    var ui = new UserInformations()
                    {
                        Guid = Guid.NewGuid()
                    };
                    var str = JsonConvert.SerializeObject(ui);
                    if (!await _storageService.SetUserInformations(str))
                        LogHelper.Instance.Log(LogLevel.FatalError, this, "Cannot save User Informations");
                    return ui;
                }
                return JsonConvert.DeserializeObject<UserInformations>(res);
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex, this);
            }
            return null;
        }

        public async Task<bool> SaveUserInformations(UserInformations userInformations)
        {

            try
            {
                var str = JsonConvert.SerializeObject(userInformations);
                return await _storageService.SetUserInformations(str);
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
                    new Beer() {Guid = Guid.Empty,Posted = true,Time = DateTime.Now},
                    new Beer() {Guid = Guid.Empty,Posted = true,Time = DateTime.Now},
                    new Beer() {Guid = Guid.Empty,Posted = true,Time = DateTime.Now},
                    new Beer() {Guid = Guid.Empty,Posted = true,Time = DateTime.Now},
                    new Beer() {Guid = Guid.Empty,Posted = true,Time = DateTime.Now},
                },
                Guid = Guid.NewGuid(),
                Name = "Florian"
            };
        }
    }
}
