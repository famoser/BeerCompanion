using Famoser.BeerCompanion.Data.Entities.Communication.Base;

namespace Famoser.BeerCompanion.Data.Entities.Communication.Generic
{
    public class BooleanResponse : BaseResponse
    {
        public new bool IsSuccessfull => Response && string.IsNullOrEmpty(ErrorMessage);

        public bool Response { get; set; }
    }
}
