using System;
using System.Collections.Generic;

namespace Famoser.BeerCompanion.Data.Entities
{
    public class DrinkerCycleEntity
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
