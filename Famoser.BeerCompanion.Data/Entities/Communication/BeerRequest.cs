using Famoser.BeerCompanion.Data.Entities.Communication.Generic;
using System;
using System.Collections.Generic;
using Famoser.BeerCompanion.Data.Entities.Communication.Base;
using Famoser.BeerCompanion.Data.Enums;

namespace Famoser.BeerCompanion.Data.Entities.Communication
{
    //object created from memory, not sure if still in use
    public class BeerRequest : BaseRequest
    {
        public BeerRequest(PossibleActions action, Guid guid) : base(action, guid)
        {

        }

        public List<BeerEntity> Beers { get; set; }
    }
}
