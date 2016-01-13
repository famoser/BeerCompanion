using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;

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
            get { return Beers.LastOrDefault()?.DrinkTime; }
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
