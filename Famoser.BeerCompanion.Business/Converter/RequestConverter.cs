using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Business.Models;
using Famoser.BeerCompanion.Common.Framework.Singleton;
using Famoser.BeerCompanion.Data.Entities;
using Famoser.BeerCompanion.Data.Entities.Communication;
using Famoser.BeerCompanion.Data.Enums;

namespace Famoser.BeerCompanion.Business.Converter
{
    public class RequestConverter : SingletonBase<RequestConverter>
    {
        public BeerRequest ConvertToBeerRequest(Guid userId, PossibleActions actionName, List<Beer> beers)
        {
            var coll = new BeerRequest(actionName, userId)
            {
                Beers = new List<BeerEntity>()
            };
            foreach (var beer in beers)
            {
                coll.Beers.Add(new BeerEntity() { DrinkTime = beer.DrinkTime, Guid = beer.Guid });
            }
            return coll;
        }

        public DrinkerCycleRequest ConvertToDrinkerCycleRequest(Guid userGuid, PossibleActions actionName, string name)
        {
            return new DrinkerCycleRequest(actionName,userGuid) { Name = name };
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
