using System.Linq;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Data.Entities.Communication;
using Famoser.BeerCompanion.Data.Enums;
using Famoser.BeerCompanion.Data.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Famoser.BeerCompanion.UnitTests.API
{
    [TestClass]
    public class DrinkerCycleRequests
    {
        [TestMethod]
        public void DoLiveCycle()
        {
            Task.Run(async () =>
            {
                //arrange
                await ApiTestHelper.CreateTestUser(ApiTestHelper.TestUserGuid);
                await ApiTestHelper.CreateTestUser(ApiTestHelper.TestUserGuid2);
                var ds = new DataService();
                var exists = new DrinkerCycleRequest(PossibleActions.Exists, ApiTestHelper.TestUserGuid) { Name = ApiTestHelper.TestCycleGuid.ToString() };
                var add1 = new DrinkerCycleRequest(PossibleActions.Add, ApiTestHelper.TestUserGuid) { Name = ApiTestHelper.TestCycleGuid.ToString() };
                var remove1 = new DrinkerCycleRequest(PossibleActions.Remove, ApiTestHelper.TestUserGuid) { Name = ApiTestHelper.TestCycleGuid.ToString() };
                var add2 = new DrinkerCycleRequest(PossibleActions.Add, ApiTestHelper.TestUserGuid2) { Name = ApiTestHelper.TestCycleGuid.ToString() };
                var removeForeign2 = new DrinkerCycleRequest(PossibleActions.RemoveForeign, ApiTestHelper.TestUserGuid) { Name = ApiTestHelper.TestCycleGuid.ToString(), AuthGuid = ApiTestHelper.TestUserGuid2 };
                var remove2 = new DrinkerCycleRequest(PossibleActions.Remove, ApiTestHelper.TestUserGuid2) { Name = ApiTestHelper.TestCycleGuid.ToString() };
                var auth2 = new DrinkerCycleRequest(PossibleActions.Autheticate, ApiTestHelper.TestUserGuid) { Name = ApiTestHelper.TestCycleGuid.ToString(), AuthGuid = ApiTestHelper.TestUserGuid2 };
                var invalidAuth2 = new DrinkerCycleRequest(PossibleActions.Autheticate, ApiTestHelper.TestUserGuid2) { Name = ApiTestHelper.TestCycleGuid.ToString(), AuthGuid = ApiTestHelper.TestUserGuid2 };
                var deauth2 = new DrinkerCycleRequest(PossibleActions.Deautheticate, ApiTestHelper.TestUserGuid) { Name = ApiTestHelper.TestCycleGuid.ToString(), AuthGuid = ApiTestHelper.TestUserGuid2 };
                var invalidDeauth2 = new DrinkerCycleRequest(PossibleActions.Deautheticate, ApiTestHelper.TestUserGuid2) { Name = ApiTestHelper.TestCycleGuid.ToString(), AuthGuid = ApiTestHelper.TestUserGuid };

                //act
                //check if already exists;
                var res = await ds.PostDrinkerCycle(exists);
                ApiAssertHelper.CheckBooleanResponseForFalse(res);

                //add 1;
                res = await ds.PostDrinkerCycle(add1);
                ApiAssertHelper.CheckBooleanResponse(res);

                //check if exists;
                res = await ds.PostDrinkerCycle(exists);
                ApiAssertHelper.CheckBooleanResponse(res);

                //check if 1 is authenticated
                var cycles = await ds.GetDrinkerCycle(ApiTestHelper.TestUserGuid);
                CheckForEmptyCycle(cycles);

                //add 2;
                res = await ds.PostDrinkerCycle(add2);
                ApiAssertHelper.CheckBooleanResponse(res);

                //check if 2 is not authenticated
                cycles = await ds.GetDrinkerCycle(ApiTestHelper.TestUserGuid);
                CheckForNotAuthenticated(cycles);

                //invalid authenticate 2
                res = await ds.PostDrinkerCycle(invalidAuth2);
                ApiAssertHelper.CheckBooleanResponseForFalse(res);

                //check if 2 is not authenticated
                cycles = await ds.GetDrinkerCycle(ApiTestHelper.TestUserGuid);
                CheckForNotAuthenticated(cycles);

                //valid authenticate 2
                res = await ds.PostDrinkerCycle(auth2);
                ApiAssertHelper.CheckBooleanResponse(res);

                //check if 2 is authenticated
                cycles = await ds.GetDrinkerCycle(ApiTestHelper.TestUserGuid);
                CheckForAuthenticated(cycles);
                
                //removeforeign 2
                res = await ds.PostDrinkerCycle(removeForeign2);
                ApiAssertHelper.CheckBooleanResponse(res);

                //check if 2 is removed
                cycles = await ds.GetDrinkerCycle(ApiTestHelper.TestUserGuid);
                CheckForEmptyCycle(cycles);

                //add 2 again & authenticate
                res = await ds.PostDrinkerCycle(add2);
                ApiAssertHelper.CheckBooleanResponse(res);
                res = await ds.PostDrinkerCycle(auth2);
                ApiAssertHelper.CheckBooleanResponse(res);

                //valid deauthenticate 2
                res = await ds.PostDrinkerCycle(deauth2);
                ApiAssertHelper.CheckBooleanResponse(res);

                //check if 2 is not authenticated
                cycles = await ds.GetDrinkerCycle(ApiTestHelper.TestUserGuid);
                CheckForNotAuthenticated(cycles);

                //invalid deauthenticate 2
                res = await ds.PostDrinkerCycle(invalidDeauth2);
                ApiAssertHelper.CheckBooleanResponseForFalse(res);
                
                //remove 2 again
                res = await ds.PostDrinkerCycle(remove2);
                ApiAssertHelper.CheckBooleanResponse(res);

                //check if 2 is not in group
                cycles = await ds.GetDrinkerCycle(ApiTestHelper.TestUserGuid);
                CheckForEmptyCycle(cycles);

                //add 1;
                res = await ds.PostDrinkerCycle(remove1);
                ApiAssertHelper.CheckBooleanResponse(res);

                //check if group gone again;
                res = await ds.PostDrinkerCycle(exists);
                ApiAssertHelper.CheckBooleanResponseForFalse(res);

                //clean
                await ApiTestHelper.DeleteTestUser(ApiTestHelper.TestUserGuid);
                await ApiTestHelper.DeleteTestUser(ApiTestHelper.TestUserGuid2);
            }).GetAwaiter().GetResult();
        }

        private void CheckForEmptyCycle(DrinkerCycleResponse cycles)
        {
            ApiAssertHelper.CheckBaseResponse(cycles);
            //check if correct cycle
            Assert.IsTrue(cycles.DrinkerCycles.Count == 1);
            Assert.IsTrue(cycles.DrinkerCycles[0].Name == ApiTestHelper.TestCycleGuid.ToString());

            //check if correct content
            Assert.IsTrue(cycles.DrinkerCycles[0].IsAuthenticated);
            Assert.IsTrue(cycles.Drinkers == null || !cycles.Drinkers.Any());
        }

        private void CheckForValidCycles(DrinkerCycleResponse cycles)
        {
            ApiAssertHelper.CheckBaseResponse(cycles);
            //check if correct cycle
            Assert.IsTrue(cycles.DrinkerCycles.Count == 1);
            Assert.IsTrue(cycles.DrinkerCycles[0].Name == ApiTestHelper.TestCycleGuid.ToString());

            //check if correct content
            Assert.IsTrue(cycles.DrinkerCycles[0].IsAuthenticated);
            Assert.IsTrue(cycles.Drinkers != null && cycles.Drinkers.Any());
        }

        private void CheckForNotAuthenticated(DrinkerCycleResponse cycles)
        {
            CheckForValidCycles(cycles);
            Assert.IsTrue(cycles.Drinkers[0].AuthDrinkerCycles == null || !cycles.Drinkers[0].AuthDrinkerCycles.Any());
            Assert.IsTrue(cycles.Drinkers[0].NonAuthDrinkerCycles.Any(d => d == cycles.DrinkerCycles[0].Guid));
        }

        private void CheckForAuthenticated(DrinkerCycleResponse cycles)
        {
            CheckForValidCycles(cycles);
            Assert.IsTrue(cycles.Drinkers[0].NonAuthDrinkerCycles == null || !cycles.Drinkers[0].NonAuthDrinkerCycles.Any());
            Assert.IsTrue(cycles.Drinkers[0].AuthDrinkerCycles.Any(d => d == cycles.DrinkerCycles[0].Guid));
        }
    }
}
