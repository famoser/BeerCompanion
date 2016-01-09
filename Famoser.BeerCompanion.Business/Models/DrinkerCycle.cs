using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Famoser.BeerCompanion.Business.Models
{
    public class DrinkerCycle
    {
        public string Name { get; set; }
        public ObservableCollection<Drinker> BeerDrinkers { get; set; }
    }
}
