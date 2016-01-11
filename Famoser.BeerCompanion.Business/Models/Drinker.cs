using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Famoser.BeerCompanion.Business.Models
{
    [DataContract]
    public class Drinker
    {
        public Drinker()
        {
            DrinkerCycles = new ObservableCollection<DrinkerCycle>();
        }

        [DataMember]
        public Guid Guid { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Color { get; set; }

        [DataMember]
        public int TotalBeers { get; set; }
        [DataMember]
        public DateTime LastBeer { get; set; }

        public ObservableCollection<DrinkerCycle> DrinkerCycles { get; set; }

        [DataMember]
        public List<Guid> AuthDrinkerCycles { get; set; }
        [DataMember]
        public List<Guid> NonAuthDrinkerCycles { get; set; }
    }
}
