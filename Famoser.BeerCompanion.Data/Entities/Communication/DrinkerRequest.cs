using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Data.Entities.Communication.Base;
using Famoser.BeerCompanion.Data.Entities.Communication.Generic;
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
