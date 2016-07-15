using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Famoser.BeerCompanion.Data.Entities.Communication.Base;
using Famoser.BeerCompanion.Data.Enums;

namespace Famoser.BeerCompanion.Data.Entities.Communication
{
    [DataContract]
    public class BeerRequest : BaseRequest
    {
        public BeerRequest(PossibleActions action, Guid guid) : base(action, guid)
        { }

        [DataMember]
        public List<BeerEntity> Beers { get; set; }

        [DataMember]
        public int ExpectedCount { get; set; }
    }
}
