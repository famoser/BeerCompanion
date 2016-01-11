using System.Collections.Generic;

namespace Famoser.BeerCompanion.Data.Entities.Communication
{
    public class DrinkerCycleResponse
    {
        public List<DrinkerEntity> Drinkers { get; set; }
        public List<DrinkerCycleEntity> DrinkerCycles { get; set; }
    }
}
