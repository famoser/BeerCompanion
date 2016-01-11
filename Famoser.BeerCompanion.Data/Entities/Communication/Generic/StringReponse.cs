using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Data.Entities.Communication.Base;

namespace Famoser.BeerCompanion.Data.Entities.Communication.Generic
{
    public class StringReponse : BaseResponse
    {
        public string Response { get; set; }
    }
}
