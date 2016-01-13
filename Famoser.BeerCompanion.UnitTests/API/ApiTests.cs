using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Data;
using Famoser.BeerCompanion.Data.Entities;
using Famoser.BeerCompanion.Data.Entities.Communication;
using Famoser.BeerCompanion.Data.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Famoser.BeerCompanion.UnitTests.API
{
    [TestClass]
    public class ApiTestHelper
    {
        public static Guid TestUserGuid = Guid.Parse("302f864d-0842-473d-bab6-28dfd1902e5e");
        public static Guid TestUserGuid2 = Guid.Parse("302f864e-0842-473d-bab6-28dfd1902e5e");
        public static Guid TestUserGuid3 = Guid.Parse("302f864f-0842-473d-bab6-28dfd1902e5e");
        public static Guid TestUser = Guid.Parse("7ec51dcc-3c00-41a5-845b-3192d30720e6");
        
        public static Guid TestCycleGuid = Guid.Parse("302f864a-0842-473d-bab6-28dfd1902e5e");
        public static string TestGroup = "302f864a";

        public static async Task CreateTestUser(Guid guid)
        {
            var ds = new DataService();
            //arrange
            var usrInfo = new UserInformationEntity()
            {
                Color = "AF56EB",
                Name = "TestUser"
            };
            var updateDrinkRequest = new DrinkerRequest(PossibleActions.Update, guid)
            {
                UserInformations = usrInfo
            };
            ApiAssertHelper.CheckBooleanResponse(await ds.PostDrinker(updateDrinkRequest));
        }

        public static async Task DeleteTestUser(Guid guid)
        {
            var ds = new DataService();
            var deleteDrinkRequest = new DrinkerRequest(PossibleActions.Remove, guid);
            ApiAssertHelper.CheckBooleanResponse(await ds.PostDrinker(deleteDrinkRequest));
        }

        [TestMethod]
        public void ApiOnline()
        {
            Task.Run(async () =>
            {
                var ds = new DataService();
                Assert.IsTrue(await ds.ApiOnline());
            });
        }
    }
}
