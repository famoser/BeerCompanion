using System.Collections.Generic;
using Famoser.BeerCompanion.Data.Entities.Communication.Base;
using Famoser.BeerCompanion.Data.Entities.Communication.Generic;

namespace Famoser.BeerCompanion.Data.Entities.Communication
{
    public class DrinkerCycleResponse : BaseResponse
    {
        public List<DrinkerEntity> Drinkers { get; set; }
        public List<DrinkerCycleEntity> DrinkerCycles { get; set; }
    }
}
