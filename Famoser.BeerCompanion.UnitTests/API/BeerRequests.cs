using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Data.Entities;
using Famoser.BeerCompanion.Data.Entities.Communication;
using Famoser.BeerCompanion.Data.Enums;
using Famoser.BeerCompanion.Data.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Famoser.BeerCompanion.UnitTests.API
{
    [TestClass]
    public class BeerRequests
    {
        [TestMethod]
        public void DoLiveCycle()
        {
            Task.Run(async () =>
            {
                //arrange
                await ApiTestHelper.CreateTestUser(ApiTestHelper.TestUserGuid);
                var ds = new DataService();
                var beer1 = new BeerEntity()
                {
                    Guid = Guid.NewGuid(),
                    DrinkTime = DateTime.Now - TimeSpan.FromDays(2)
                };
                var beer2 = new BeerEntity()
                {
                    Guid = Guid.NewGuid(),
                    DrinkTime = DateTime.Now - TimeSpan.FromDays(1)
                };
                var beer3 = new BeerEntity()
                {
                    Guid = Guid.NewGuid(),
                    DrinkTime = DateTime.Now
                };

                var add = new BeerRequest(PossibleActions.Add, ApiTestHelper.TestUserGuid)
                {
                    Beers = new List<BeerEntity>()
                    {
                        beer1, beer2, beer3
                    }
                };
                var remove1 = new BeerRequest(PossibleActions.Remove, ApiTestHelper.TestUserGuid)
                {
                    Beers = new List<BeerEntity>()
                    {
                        beer1, beer3
                    }
                };
                var remove2 = new BeerRequest(PossibleActions.Remove, ApiTestHelper.TestUserGuid)
                {
                    Beers = new List<BeerEntity>()
                    {
                        beer2
                    }
                };

                //act
                //check if 0 beers
                var beers = await ds.GetBeers(ApiTestHelper.TestUserGuid);
                ApiAssertHelper.CheckBaseResponse(beers);
                Assert.IsTrue(beers.Beers == null || !beers.Beers.Any());

                //add beers;
                var res = await ds.PostBeer(add);
                ApiAssertHelper.CheckBooleanResponse(res);
                
                //check if 3 beers
                beers = await ds.GetBeers(ApiTestHelper.TestUserGuid);
                ApiAssertHelper.CheckBaseResponse(beers);
                Assert.IsTrue(beers.Beers != null && beers.Beers.Count == 3);

                //remove 2 beers;
                res = await ds.PostBeer(remove1);
                ApiAssertHelper.CheckBooleanResponse(res);

                //check if 1 beer, check Date
                beers = await ds.GetBeers(ApiTestHelper.TestUserGuid);
                ApiAssertHelper.CheckBaseResponse(beers);
                Assert.IsTrue(beers.Beers != null && beers.Beers.Count == 1);
                Assert.IsTrue(beers.Beers[0].Guid == beer2.Guid);
                Assert.IsTrue(beers.Beers[0].DrinkTime - beer2.DrinkTime < TimeSpan.FromSeconds(1));

                //remove 2 invalid beers;
                res = await ds.PostBeer(remove1);
                ApiAssertHelper.CheckBooleanResponse(res);

                //remove 1 beer left;
                res = await ds.PostBeer(remove2);
                ApiAssertHelper.CheckBooleanResponse(res);

                //check if 0 beers
                beers = await ds.GetBeers(ApiTestHelper.TestUserGuid);
                ApiAssertHelper.CheckBaseResponse(beers);
                Assert.IsTrue(beers.Beers == null || !beers.Beers.Any());
                
                //clean
                await ApiTestHelper.DeleteTestUser(ApiTestHelper.TestUserGuid);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void TestSync()
        {
            Task.Run(async () =>
            {
                //arrange
                await ApiTestHelper.CreateTestUser(ApiTestHelper.TestUserGuid);
                var ds = new DataService();
                var beer1 = new BeerEntity()
                {
                    Guid = Guid.NewGuid(),
                    DrinkTime = DateTime.Now - TimeSpan.FromDays(2)
                };
                var beer2 = new BeerEntity()
                {
                    Guid = Guid.NewGuid(),
                    DrinkTime = DateTime.Now - TimeSpan.FromDays(1)
                };
                var beer3 = new BeerEntity()
                {
                    Guid = Guid.NewGuid(),
                    DrinkTime = DateTime.Now
                };
                var beer4 = new BeerEntity()
                {
                    Guid = Guid.NewGuid(),
                    DrinkTime = DateTime.Now + TimeSpan.FromDays(1)
                };

                var add = new BeerRequest(PossibleActions.Add, ApiTestHelper.TestUserGuid)
                {
                    Beers = new List<BeerEntity>()
                    {
                        beer1, beer2, beer3
                    }
                };
                var remove = new BeerRequest(PossibleActions.Remove, ApiTestHelper.TestUserGuid)
                {
                    Beers = new List<BeerEntity>()
                    {
                        beer1, beer3, beer2
                    }
                };

                var correct1 = new BeerRequest(PossibleActions.Sync, ApiTestHelper.TestUserGuid)
                {
                    Beers = new List<BeerEntity>()
                    {
                        beer3
                    }
                };
                var correct2 = new BeerRequest(PossibleActions.Sync, ApiTestHelper.TestUserGuid)
                {
                    Beers = new List<BeerEntity>()
                    {
                        beer3, beer2, beer1
                    }
                };
                var false1 = new BeerRequest(PossibleActions.Sync, ApiTestHelper.TestUserGuid)
                {
                    Beers = new List<BeerEntity>()
                    {
                        beer1, beer2
                    }
                };
                var false2 = new BeerRequest(PossibleActions.Sync, ApiTestHelper.TestUserGuid)
                {
                    Beers = new List<BeerEntity>()
                    {

                    }
                };
                var false3 = new BeerRequest(PossibleActions.Sync, ApiTestHelper.TestUserGuid)
                {
                    Beers = new List<BeerEntity>()
                    {
                        beer1, beer2,beer4
                    }
                };
                var res = await ds.PostBeer(add);
                ApiAssertHelper.CheckBooleanResponse(res);


                //act
                res = await ds.PostBeer(correct1);
                ApiAssertHelper.CheckBooleanResponse(res);

                res = await ds.PostBeer(correct2);
                ApiAssertHelper.CheckBooleanResponse(res);

                res = await ds.PostBeer(false1);
                ApiAssertHelper.CheckBooleanResponseForFalse(res);

                res = await ds.PostBeer(false2);
                ApiAssertHelper.CheckBooleanResponseForFalse(res);

                res = await ds.PostBeer(false3);
                ApiAssertHelper.CheckBooleanResponseForFalse(res);

                //clean
                await ds.PostBeer(remove);
                await ApiTestHelper.DeleteTestUser(ApiTestHelper.TestUserGuid);
            }).GetAwaiter().GetResult();




        }
    }
}
