using System;
using System.Collections.Generic;

namespace Famoser.BeerCompanion.Data.Entities
{
    public class DrinkerEntity
    {
        public string Name { get; set; }
        public string Color { get; set; }

        public int TotalBeers { get; set; }
        public DateTime LastBeer { get; set; }

        public List<Guid> AuthDrinkerCycles { get; set; }
        public List<Guid> NonAuthDrinkerCycles { get; set; }
    }
}
