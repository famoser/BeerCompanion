using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Data;
using Famoser.BeerCompanion.Data.Entities.Communication;
using Famoser.BeerCompanion.Data.Enums;
using Famoser.BeerCompanion.Data.Services;
using Famoser.BeerCompanion.UnitTests.API;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Famoser.BeerCompanion.UnitTests.TestCases
{
    [TestClass]
    public class GroupBehaviour : ITestCase
    {
        [TestMethod]
        [Ignore]
        public void Prepare()
        {
            //you are added to
            var f = ApiTestHelper.TestGroup;
            //and some other dudes
            Task.Run(async () =>
            {
                //arrange
                await ApiTestHelper.CreateTestUser(ApiTestHelper.TestUserGuid);
                await ApiTestHelper.CreateTestUser(ApiTestHelper.TestUserGuid2);
                await ApiTestHelper.CreateTestUser(ApiTestHelper.TestUserGuid3);
                var ds = new DataService();
                var add0 = new DrinkerCycleRequest(PossibleActions.Add, ApiTestHelper.TestUser) { Name = ApiTestHelper.TestGroup };
                var add1 = new DrinkerCycleRequest(PossibleActions.Add, ApiTestHelper.TestUserGuid) { Name = ApiTestHelper.TestGroup };
                var add2 = new DrinkerCycleRequest(PossibleActions.Add, ApiTestHelper.TestUserGuid2) { Name = ApiTestHelper.TestGroup };
                var add3 = new DrinkerCycleRequest(PossibleActions.Add, ApiTestHelper.TestUserGuid3) { Name = ApiTestHelper.TestGroup };
                var check1 = new DrinkerCycleRequest(PossibleActions.Exists, ApiTestHelper.TestUserGuid3) { Name = ApiTestHelper.TestGroup };
                
                //add0
                var res = await ds.PostDrinkerCycle(add0);
                ApiAssertHelper.CheckBooleanResponse(res);

                //act
                //check if user is in group
                res = await ds.PostDrinkerCycle(check1);
                ApiAssertHelper.CheckBooleanResponse(res);

                //check if group is empty except drinker
                var cylces = await ds.GetDrinkerCycle(ApiTestHelper.TestUser);
                ZeroMembers(cylces);

                //add1
                res = await ds.PostDrinkerCycle(add1);
                ApiAssertHelper.CheckBooleanResponse(res);

                //add2
                res = await ds.PostDrinkerCycle(add2);
                ApiAssertHelper.CheckBooleanResponse(res);

                //add3
                res = await ds.PostDrinkerCycle(add3);
                ApiAssertHelper.CheckBooleanResponse(res);

                //check if three drinkers are returned
                cylces = await ds.GetDrinkerCycle(ApiTestHelper.TestUser);
                ThreeMembers(cylces);
            }).GetAwaiter().GetResult();
        }

        private void ThreeMembers(DrinkerCycleResponse resp)
        {
            ApiAssertHelper.CheckBaseResponse(resp);
            Assert.IsNotNull(resp.DrinkerCycles);
            Assert.IsTrue(resp.DrinkerCycles.Any(d => d.Name == ApiTestHelper.TestGroup));
            var group = resp.DrinkerCycles.FirstOrDefault(d => d.Name == ApiTestHelper.TestGroup);
            Assert.IsTrue(resp.Drinkers.Count(d => d.NonAuthDrinkerCycles.Any(b => b == group.Guid)) == 3);
        }

        private void ZeroMembers(DrinkerCycleResponse resp)
        {
            ApiAssertHelper.CheckBaseResponse(resp);
            Assert.IsNotNull(resp.DrinkerCycles);
            Assert.IsTrue(resp.DrinkerCycles.Any(d => d.Name == ApiTestHelper.TestGroup));
            var group = resp.DrinkerCycles.FirstOrDefault(d => d.Name == ApiTestHelper.TestGroup);
            Assert.IsTrue(resp.Drinkers == null || resp.Drinkers.Count(d => d.NonAuthDrinkerCycles.Any(b => b == group.Guid)) == 0);
        }

        [TestMethod]
        [Ignore]
        public void Test()
        {
            //authenticate all users in the group in the application, afterwards run method
            Task.Run(async () =>
            {
                //arrange
                var ds = new DataService();

                //act
                //check1
                var res = await ds.GetDrinkerCycle(ApiTestHelper.TestUserGuid);
                IsAuthenticated(res);

                //check2
                res = await ds.GetDrinkerCycle(ApiTestHelper.TestUserGuid2);
                IsAuthenticated(res);

                //check3
                res = await ds.GetDrinkerCycle(ApiTestHelper.TestUserGuid3);
                IsAuthenticated(res);
            }).GetAwaiter().GetResult();
        }

        private void IsAuthenticated(DrinkerCycleResponse resp)
        {
            ApiAssertHelper.CheckBaseResponse(resp);
            Assert.IsNotNull(resp.DrinkerCycles);
            Assert.IsTrue(resp.DrinkerCycles.Any(d => d.Name == ApiTestHelper.TestGroup));
            Assert.IsTrue(resp.DrinkerCycles.First(d => d.Name == ApiTestHelper.TestGroup).IsAuthenticated);
        }

        [TestMethod]
        [Ignore]
        public void Run()
        {
            //remove all test users from group and run this method
            Task.Run(async () =>
            {
                //arrange
                var ds = new DataService();

                //act
                //check1
                var res = await ds.GetDrinkerCycle(ApiTestHelper.TestUserGuid);
                IsNotInGroup(res);

                //check2
                res = await ds.GetDrinkerCycle(ApiTestHelper.TestUserGuid2);
                IsNotInGroup(res);

                //check3
                res = await ds.GetDrinkerCycle(ApiTestHelper.TestUserGuid3);
                IsNotInGroup(res);
            }).GetAwaiter().GetResult();
        }

        private void IsNotInGroup(DrinkerCycleResponse resp)
        {
            ApiAssertHelper.CheckBaseResponse(resp);
            Assert.IsNotNull(resp.DrinkerCycles);
            Assert.IsTrue(resp.DrinkerCycles.All(d => d.Name != ApiTestHelper.TestGroup));
        }

        [TestMethod]
        [Ignore]
        public void Clean()
        {
            //authenticate all users in the group in the application, afterwards run method
            Task.Run(async () =>
            {
                //clean
                await ApiTestHelper.DeleteTestUser(ApiTestHelper.TestUserGuid);
                await ApiTestHelper.DeleteTestUser(ApiTestHelper.TestUserGuid2);
                await ApiTestHelper.DeleteTestUser(ApiTestHelper.TestUserGuid3);
            }).GetAwaiter().GetResult();
        }
    }
}
