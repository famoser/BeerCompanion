using System;
using System.Collections.Generic;

namespace Famoser.BeerCompanion.Data.Entities.Communication
{
    //object created from memory, not sure if still in use
    public class BeerRequest
    {
        public Guid Guid { get; set; }
        public List<BeerEntity> Beers { get; set; }
        public string Action { get; set; }
    }
}
