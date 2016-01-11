namespace Famoser.BeerCompanion.Data.Entities.Communication.Base
{
    public class BaseResponse 
    {
        public bool IsSuccessfull => ErrorMessage == null;

        public string ErrorMessage { get; set; }
    }
}
