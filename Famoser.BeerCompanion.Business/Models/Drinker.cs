using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

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

        public override int GetTotalBeers
        {
            get { return TotalBeers; }
        }

        public override DateTime? GetLastBeer
        {
            get { return LastBeer; }
        }

        [DataMember]
        public List<Guid> AuthDrinkerCycleGuids { get; set; }
        [DataMember]
        public List<Guid> NonAuthDrinkerCycleGuids { get; set; }
    }
}
