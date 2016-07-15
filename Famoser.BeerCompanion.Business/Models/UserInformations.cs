using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace Famoser.BeerCompanion.Business.Models
{
    [DataContract]
    public class UserInformations : Person
    {
        public UserInformations()
        {
            Beers = new ObservableCollection<Beer>();
        }
        
        private bool _firstName;
        public bool FirstTime
        {
            get { return _firstName; }
            set { Set(ref _firstName, value); }
        }

        public override int GetTotalBeers
        {
            get { return Beers.Count; }
        }

        public override DateTime? GetLastBeer
        {
            get 
			{ 
				var lastbeer = Beers.LastOrDefault ();
				if (lastbeer != null)
					return lastbeer.DrinkTime;
				return null; 
			}
        }

        private ObservableCollection<Beer> _beers;
        public ObservableCollection<Beer> Beers
        {
            get { return _beers; }
            set
            {
                Set(ref _beers, value);
            }
        }
    }
}
