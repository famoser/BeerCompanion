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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Famoser.BeerCompanion.UnitTests.API
{
    [TestClass]
    public class DrinkerRequests
    {
        private static Guid _testGuid = Guid.NewGuid();
        [TestMethod]
        public async Task UpdateRequest()
        {
            //arrange
            var usrInfo = new UserInformationEntity()
            {
                Color = "AF56EB",
                Name = "TestUser"
            };
            var updateDrinkRequest = new DrinkerRequest(PossibleActions.Update, _testGuid)
            {
                UserInformations = usrInfo
            };
            var existDrinkRequest = new DrinkerRequest(PossibleActions.Exists, _testGuid)
            {
                UserInformations = usrInfo
            };
            var removeDrinkRequest = new DrinkerRequest(PossibleActions.Remove, _testGuid)
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
            var drinker = await ds.GetDrinker(_testGuid);
            CheckGetDrinkerResponse(drinker, updateDrinkRequest);

            //update
            updateDrinkRequest.UserInformations.Name = "NewName";
            updateDrinkRequest.UserInformations.Color = "NewColor";
            res = await ds.PostDrinker(updateDrinkRequest);
            ApiAssertHelper.CheckBooleanResponse(res);

            //check for updated values
            drinker = await ds.GetDrinker(_testGuid);
            CheckGetDrinkerResponse(drinker, updateDrinkRequest);

            //delete drinker again
            res = await ds.PostDrinker(removeDrinkRequest);
            ApiAssertHelper.CheckBooleanResponse(res);
            drinker = await ds.GetDrinker(_testGuid);
            ApiAssertHelper.CheckBaseResponse(drinker);
            Assert.IsNull(drinker.Drinker);
        }

        private void CheckGetDrinkerResponse(DrinkerResponse resp, DrinkerRequest requ)
        {
            ApiAssertHelper.CheckBaseResponse(resp);
            Assert.IsNotNull(resp.Drinker);
            Assert.IsTrue(resp.Drinker.Name == requ.UserInformations.Name);
            Assert.IsTrue(resp.Drinker.Color == requ.UserInformations.Color);
        }

        [TestMethod]
        public async Task RemoveRequest()
        {
            //check also for clean database

        }
    }
}
