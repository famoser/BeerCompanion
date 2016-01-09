using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Famoser.BeerCompanion.Data.Entities
{
    public class DrinkerCycleCommunication
    {
        public string Name { get; set; }
        public Guid Guid { get; set; }

        public string Action { get; set; }
    }
}
