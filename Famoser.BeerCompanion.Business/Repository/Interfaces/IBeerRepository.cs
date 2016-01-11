using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Business.Models;

namespace Famoser.BeerCompanion.Business.Repository.Interfaces
{
    public interface IBeerRepository
    {
        Task<ObservableCollection<Beer>> SyncBeers(ObservableCollection<Beer> beers, Guid userGuid);
    }
}
