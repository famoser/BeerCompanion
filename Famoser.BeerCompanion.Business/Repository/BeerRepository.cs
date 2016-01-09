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
        private IDataService _dataService;

        public BeerRepository(IDataService dataService)
        {
            _dataService = dataService;
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
            return null;
        }
    }
}
