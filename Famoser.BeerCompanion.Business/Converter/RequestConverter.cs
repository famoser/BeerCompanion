using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Business.Models;
using Famoser.BeerCompanion.Common.Framework.Singleton;
using Famoser.BeerCompanion.Data.Entities;
using Famoser.BeerCompanion.Data.Entities.Communication;

namespace Famoser.BeerCompanion.Business.Converter
{
    public class RequestConverter : SingletonBase<RequestConverter>
    {
        public BeerRequest ConvertToBeerAddRequest(Guid userId, List<Beer> beers)
        {
            var requ = ConstructRequest(userId, beers);
            requ.Action = "add";
            return requ;
        }

        public BeerRequest ConvertToBeerRemoveRequest(Guid userId, List<Beer> beers)
        {
            var requ = ConstructRequest(userId, beers);
            requ.Action = "remove";
            return requ;
        }

        private BeerRequest ConstructRequest(Guid userId, List<Beer> beers)
        {
            var coll = new BeerRequest
            {
                Guid = userId,
                Beers = new List<BeerEntity>()
            };
            foreach (var beer in beers)
            {
                coll.Beers.Add(new BeerEntity() { DrinkTime = beer.DrinkTime, Guid = beer.Guid });
            }
            return coll;
        }

        public DrinkerCycleRequest ConvertToDrinkerCycleRequest(string name, Guid userGuid, string actionName)
        {
            return new DrinkerCycleRequest() { Action = actionName, Guid = userGuid, Name = name };
        }
    }
}
