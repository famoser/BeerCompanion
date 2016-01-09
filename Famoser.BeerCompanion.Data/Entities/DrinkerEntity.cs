using System;
using System.Collections.Generic;

namespace Famoser.BeerCompanion.Data.Entities
{
    public class DrinkerEntity
    {
        public string Name { get; set; }

        public int TotalBeers { get; set; }
        public DateTime LastBeer { get; set; }
        public Dictionary<Guid, bool> DrinkerCycles { get; set; }
    }
}
