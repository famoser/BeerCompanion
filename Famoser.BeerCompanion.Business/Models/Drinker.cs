using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Famoser.BeerCompanion.Business.Models
{
    public class Drinker
    {
        public string Name { get; set; }
        
        public int TotalBeers { get; set; }
        public DateTime LastBeer { get; set; }

        [JsonIgnore]
        public ObservableCollection<DrinkerCycle> DrinkerCycles { get; set; }
    }
}
