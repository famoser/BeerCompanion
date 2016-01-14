using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Data;
using Famoser.BeerCompanion.Data.Entities;
using Famoser.BeerCompanion.Data.Entities.Communication;
using Famoser.BeerCompanion.Data.Entities.Communication.Generic;
using Famoser.BeerCompanion.Data.Enums;
using Famoser.BeerCompanion.Data.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Famoser.BeerCompanion.UnitTests.API
{
    [TestClass]
    public class DrinkerRequests
    {
        [TestMethod]
        public void DoLiveCycle()
        {
            Task.Run(async () =>
            {
                //arrange
                var usrInfo = new UserInformationEntity()
                {
                    Color = "AF56EB",
                    Name = "TestUser"
                };
                var updateDrinkRequest = new DrinkerRequest(PossibleActions.Update, ApiTestHelper.TestUserGuid)
                {
                    UserInformations = usrInfo
                };
                var existDrinkRequest = new DrinkerRequest(PossibleActions.Exists, ApiTestHelper.TestUserGuid)
                {
                    UserInformations = usrInfo
                };
                var removeDrinkRequest = new DrinkerRequest(PossibleActions.Remove, ApiTestHelper.TestUserGuid)
                {
                    UserInformations = usrInfo
                };
                var ds = new DataService();

                //act
                //check if already exists;
                var res = await ds.PostDrinker(existDrinkRequest);
                ApiAssertHelper.CheckBooleanResponseForFalse(res);

                //add
                res = await ds.PostDrinker(updateDrinkRequest);
                ApiAssertHelper.CheckBooleanResponse(res);

                //check for existence
                var drinker = await ds.GetDrinker(ApiTestHelper.TestUserGuid);
                CheckGetDrinkerResponse(drinker, updateDrinkRequest);

                //update
                updateDrinkRequest.UserInformations.Name = "NewName";
                updateDrinkRequest.UserInformations.Color = "NewColor";
                res = await ds.PostDrinker(updateDrinkRequest);
                ApiAssertHelper.CheckBooleanResponse(res);

                //check for updated values
                drinker = await ds.GetDrinker(ApiTestHelper.TestUserGuid);
                CheckGetDrinkerResponse(drinker, updateDrinkRequest);

                //delete drinker again
                res = await ds.PostDrinker(removeDrinkRequest);
                ApiAssertHelper.CheckBooleanResponse(res);

                //ensure Drinker is deleted;
                res = await ds.PostDrinker(existDrinkRequest);
                ApiAssertHelper.CheckBooleanResponseForFalse(res);
            }).GetAwaiter().GetResult();
        }

        private void CheckGetDrinkerResponse(DrinkerResponse resp, DrinkerRequest requ)
        {
            ApiAssertHelper.CheckBaseResponse(resp);
            Assert.IsNotNull(resp.Drinker);
            Assert.IsTrue(resp.Drinker.Name == requ.UserInformations.Name);
            Assert.IsTrue(resp.Drinker.Color == requ.UserInformations.Color);
        }
    }
}
