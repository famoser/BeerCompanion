using System;
using System.Collections.Generic;

namespace Famoser.BeerCompanion.Data.Entities
{
    public class DrinkerEntity
    {
        public string Name { get; set; }
        public string Color { get; set; }

        public int TotalBeers { get; set; }
        public string LastBeer { private get; set; }

        public DateTime LastBeerTime => DateTime.Parse(LastBeer);

        public List<Guid> AuthDrinkerCycles { get; set; }
        public List<Guid> NonAuthDrinkerCycles { get; set; }
    }
}
