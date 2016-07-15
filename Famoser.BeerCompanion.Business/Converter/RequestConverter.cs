using System;
using System.Collections.Generic;
using Famoser.BeerCompanion.Business.Models;
using Famoser.BeerCompanion.Data.Entities;
using Famoser.BeerCompanion.Data.Entities.Communication;
using Famoser.BeerCompanion.Data.Enums;
using Famoser.FrameworkEssentials.Singleton;

namespace Famoser.BeerCompanion.Business.Converter
{
    public class RequestConverter : SingletonBase<RequestConverter>
    {
        public BeerRequest ConvertToBeerRequest(Guid userId, PossibleActions actionName, List<Beer> beers, int expCount = 0)
        {
            var coll = new BeerRequest(actionName, userId)
            {
                Beers = new List<BeerEntity>(),
                ExpectedCount = expCount
            };
            foreach (var beer in beers)
            {
                coll.Beers.Add(new BeerEntity() { DrinkTime = beer.DrinkTime, Guid = beer.Guid });
            }
            return coll;
        }

        public DrinkerCycleRequest ConvertToDrinkerCycleRequest(Guid userGuid, PossibleActions actionName, string name, Guid? authGuid = null)
        {
            var res =  new DrinkerCycleRequest(actionName,userGuid) { Name = name };
            if (authGuid.HasValue)
                res.AuthGuid = authGuid.Value;
            return res;
        }

        public DrinkerRequest ConvertToDrinkerRequest(Guid userGuid, PossibleActions actionName, UserInformations ui)
        {
            return new DrinkerRequest(actionName, userGuid)
            {
                UserInformations = new UserInformationEntity()
                {
                    Name = ui.Name,
                    Color = ui.Color
                }
            };
        }
    }
}
