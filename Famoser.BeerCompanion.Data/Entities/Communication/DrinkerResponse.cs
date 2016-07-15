using System.Runtime.Serialization;
using Famoser.BeerCompanion.Data.Entities.Communication.Base;

namespace Famoser.BeerCompanion.Data.Entities.Communication
{
    [DataContract]
    public class DrinkerResponse : BaseResponse
    {
        [DataMember]
        public DrinkerEntity Drinker { get; set; }
    }
}
