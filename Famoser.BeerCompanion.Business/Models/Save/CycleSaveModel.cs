using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Famoser.BeerCompanion.Business.Models.Save
{
    public class CycleSaveModel
    {
        public ObservableCollection<Drinker> Drinkers { get; set; }
        public ObservableCollection<DrinkerCycle> DrinkerCycles { get; set; }
    }
}
