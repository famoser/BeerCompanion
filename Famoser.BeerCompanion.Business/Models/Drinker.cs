using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;

namespace Famoser.BeerCompanion.Business.Models
{
    [DataContract]
    public class Drinker : Person
    {
        public Drinker()
        {

        }

        [DataMember]
        public int TotalBeers { get; set; }
        [DataMember]
        public DateTime? LastBeer { get; set; }

        public override int GetTotalBeers => TotalBeers;
        public override DateTime? GetLastBeer => LastBeer;

        [DataMember]
        public List<Guid> AuthDrinkerCycleGuids { get; set; }
        [DataMember]
        public List<Guid> NonAuthDrinkerCycleGuids { get; set; }
    }
}
