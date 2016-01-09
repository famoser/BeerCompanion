using System.Collections.Generic;

namespace Famoser.BeerCompanion.Data.Entities
{
    public class ResponseCollectionEntity
    {
        public List<DrinkerEntity> Drinkers { get; set; }
        public List<DrinkerCycleEntity> DrinkerCycles { get; set; }
    }
}
