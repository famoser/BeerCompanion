using System;
using System.Runtime.Serialization;
using Famoser.BeerCompanion.Data.Entities.Communication.Base;
using Famoser.BeerCompanion.Data.Enums;

namespace Famoser.BeerCompanion.Data.Entities.Communication
{
    [DataContract]
    public class DrinkerRequest : BaseRequest
    {
        public DrinkerRequest(PossibleActions action, Guid guid) : base(action, guid)
        {
        }

        [DataMember]
        public UserInformationEntity UserInformations { get; set; }
    }
}
