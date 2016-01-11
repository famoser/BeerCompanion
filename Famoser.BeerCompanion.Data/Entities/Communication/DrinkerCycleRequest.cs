using System;
using Famoser.BeerCompanion.Data.Entities.Communication.Base;
using Famoser.BeerCompanion.Data.Entities.Communication.Generic;
using Famoser.BeerCompanion.Data.Enums;

namespace Famoser.BeerCompanion.Data.Entities.Communication
{
    public class DrinkerCycleRequest : BaseRequest
    {
        public DrinkerCycleRequest(PossibleActions action, Guid guid) : base(action, guid)
        {
        }

        public string Name { get; set; }
    }
}
