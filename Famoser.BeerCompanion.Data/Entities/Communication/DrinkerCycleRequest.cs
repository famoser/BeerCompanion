using System;

namespace Famoser.BeerCompanion.Data.Entities.Communication
{
    public class DrinkerCycleRequest
    {
        public string Name { get; set; }
        public Guid Guid { get; set; }

        public string Action { get; set; }
    }
}
