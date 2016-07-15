using System.Collections.Generic;
using System.Runtime.Serialization;
using Famoser.BeerCompanion.Data.Entities.Communication.Base;

namespace Famoser.BeerCompanion.Data.Entities.Communication
{
    [DataContract]
    public class BeerResponse : BaseResponse
    {
        [DataMember]
        public List<BeerEntity> Beers { get; set; }
    }
}
