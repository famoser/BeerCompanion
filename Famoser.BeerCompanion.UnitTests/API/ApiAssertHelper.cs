using Famoser.BeerCompanion.Data.Entities.Communication.Base;
using Famoser.BeerCompanion.Data.Entities.Communication.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Famoser.BeerCompanion.UnitTests.API
{
    public class ApiAssertHelper
    {
        public static void CheckBooleanResponse(BooleanResponse resp)
        {
            Assert.IsTrue(resp.IsSuccessfull, "Boolean Result failed with response: " + resp.Response + " and Error Message: " + resp.ErrorMessage);
        }

        public static void CheckBooleanResponseForFalse(BooleanResponse resp)
        {
            Assert.IsNull(resp.ErrorMessage, "Error message not null: " + resp.ErrorMessage);
            Assert.IsFalse(resp.Response, "Boolean Result not false as expected: " + resp.Response);
        }

        public static void CheckBaseResponse(BaseResponse resp)
        {
            Assert.IsNull(resp.ErrorMessage, "Error message not null: " + resp.ErrorMessage);
            Assert.IsTrue(resp.IsSuccessfull, "base Response not successfull as expected: " + resp.IsSuccessfull + " Error Message: " + resp.ErrorMessage);
        }
    }
}
