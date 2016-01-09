using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Business.Models;
using Famoser.BeerCompanion.Common.Framework.Singleton;
using Famoser.BeerCompanion.Data.Entities;

namespace Famoser.BeerCompanion.Business.Converter
{
    public class EntityConverter : SingletonBase<EntityConverter>
    {
        public BeerCollectionEntity ConvertToBeerCollectionEntity(Guid userId, IEnumerable<Beer> beers)
        {
            var coll = new BeerCollectionEntity
            {
                Guid = userId,
                BeerEntity = new List<BeerEntity>()
            };
            foreach (var beer in beers)
            {
                coll.BeerEntity.Add(new BeerEntity() { DrinkTime = beer.DrinkTime, Guid = beer.Guid });
            }
            return coll;
        }
    }
}
