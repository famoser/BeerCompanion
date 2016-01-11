using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Data.Entities.Communication.Base;
using Famoser.BeerCompanion.Data.Entities.Communication.Generic;

namespace Famoser.BeerCompanion.Data.Entities.Communication
{
    [DataContract]
    public class BeerResponse : BaseResponse
    {
        [DataMember]
        public List<BeerEntity> Beers { get; set; }
    }
}
