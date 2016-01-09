using System;
using System.Collections.Generic;

namespace Famoser.BeerCompanion.Data.Entities
{
    public class DrinkerCycleEntity
    {
        public string Name { get; set; }
        public List<Guid> BeerDrinker { get; set; }
    }
}
