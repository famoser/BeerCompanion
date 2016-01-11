using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Famoser.BeerCompanion.Data.Entities.Communication
{
    public class DrinkerRequest
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
    }
}
