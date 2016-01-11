using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Business.Converter;
using Famoser.BeerCompanion.Business.Models;
using Famoser.BeerCompanion.Business.Repository.Interfaces;
using Famoser.BeerCompanion.Common.Framework.Logging;
using Famoser.BeerCompanion.Data;
using Famoser.BeerCompanion.Data.Entities;
using Famoser.BeerCompanion.Data.Enums;
using Famoser.BeerCompanion.Data.Services;
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

        public async Task<ObservableCollection<Beer>> SyncBeers(ObservableCollection<Beer> beers, Guid userGuid)
        {
            try
            {
                //remove deleted & not posted
                var remove = beers.Where(b => b.DeletePending && !b.Posted);
                foreach (var beer in remove)
                {
                    beers.Remove(beer);
                }

                //remove deleted
                var deleted = beers.Where(b => b.DeletePending).ToList();
                if (deleted.Any())
                {
                    var obj = RequestConverter.Instance.ConvertToBeerRequest(userGuid,PossibleActions.Remove, deleted);
                    if ((await _dataService.PostBeer(obj)).IsSuccessfull)
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
                    var obj = RequestConverter.Instance.ConvertToBeerRequest(userGuid, PossibleActions.Add, add);
                    if ((await _dataService.PostBeer(obj)).IsSuccessfull)
                    {
                        foreach (var beer in beers)
                        {
                            beer.Posted = true;
                        }
                    }
                }

                return beers;
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex, this);
            }
            return null;
        }
    }
}
