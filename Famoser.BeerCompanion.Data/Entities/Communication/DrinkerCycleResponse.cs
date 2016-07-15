using System.Collections.Generic;
using System.Runtime.Serialization;
using Famoser.BeerCompanion.Data.Entities.Communication.Base;

namespace Famoser.BeerCompanion.Data.Entities.Communication
{
    [DataContract]
    public class DrinkerCycleResponse : BaseResponse
    {
        [DataMember]
        public List<DrinkerEntity> Drinkers { get; set; }
        [DataMember]
        public List<DrinkerCycleEntity> DrinkerCycles { get; set; }
    }
}
