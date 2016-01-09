using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Famoser.BeerCompanion.Data.Entities
{
    public class BeerCollectionEntity
    {
        public Guid Guid { get; set; }
        public List<BeerEntity> BeerEntity { get; set; }
    }
}
