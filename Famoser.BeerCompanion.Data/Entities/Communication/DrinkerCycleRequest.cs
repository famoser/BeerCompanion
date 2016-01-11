using System;
using System.Runtime.Serialization;
using Famoser.BeerCompanion.Data.Entities.Communication.Base;
using Famoser.BeerCompanion.Data.Entities.Communication.Generic;
using Famoser.BeerCompanion.Data.Enums;

namespace Famoser.BeerCompanion.Data.Entities.Communication
{
    [DataContract]
    public class DrinkerCycleRequest : BaseRequest
    {
        public DrinkerCycleRequest(PossibleActions action, Guid userGuid) : base(action, userGuid)
        {
        }

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public Guid AuthGuid { get; set; }
    }
}
