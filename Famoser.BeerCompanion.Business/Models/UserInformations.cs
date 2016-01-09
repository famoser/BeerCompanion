using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Famoser.BeerCompanion.Business.Models
{
    public class UserInformations
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }

        public int TotalBeers => Beers.Count;
        public DateTime? LastBeer => Beers.LastOrDefault()?.Time;

        public ObservableCollection<Beer> Beers { get; set; }
    }
}
