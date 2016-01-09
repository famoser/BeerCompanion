using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace Famoser.BeerCompanion.Business.Models
{
    public class DrinkerCycle : ObservableObject
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        private ObservableCollection<Drinker> _beerDrinkers;
        public ObservableCollection<Drinker> BeerDrinkers
        {
            get { return _beerDrinkers; }
            set { Set(ref _beerDrinkers, value); }
        }
    }
}
