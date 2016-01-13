using System;
using System.Collections.Generic;

namespace Famoser.BeerCompanion.Data.Entities
{
    public class DrinkerEntity
    {
        public Guid Guid { get; set; }

        public string Name { get; set; }
        public string Color { get; set; }

        public int TotalBeers { get; set; }
        public string LastBeer { private get; set; }

        public DateTime? LastBeerTime
        {
            get
            {
                DateTime res;
                if (DateTime.TryParse(LastBeer, out res))
                    return res;
                return null;
            }
        }

        public List<Guid> AuthDrinkerCycles { get; set; }
        public List<Guid> NonAuthDrinkerCycles { get; set; }
    }
}
