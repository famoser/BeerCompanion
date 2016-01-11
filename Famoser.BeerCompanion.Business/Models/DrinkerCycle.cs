using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using GalaSoft.MvvmLight;

namespace Famoser.BeerCompanion.Business.Models
{
    [DataContract]
    public class DrinkerCycle : ObservableObject
    {
        public DrinkerCycle()
        {
            AuthBeerDrinkers = new ObservableCollection<Drinker>();
            NonAuthBeerDrinkers = new ObservableCollection<Drinker>();
        }

        private Guid _guid;
        [DataMember]
        public Guid Guid
        {
            get { return _guid; }
            set { Set(ref _guid, value); }
        }

        private string _name;
        [DataMember]
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        private ObservableCollection<Drinker> _authBeerDrinkers;
        public ObservableCollection<Drinker> AuthBeerDrinkers
        {
            get { return _authBeerDrinkers; }
            set { Set(ref _authBeerDrinkers, value); }
        }

        private ObservableCollection<Drinker> _nonAuthBeerDrinkers;
        public ObservableCollection<Drinker> NonAuthBeerDrinkers
        {
            get { return _nonAuthBeerDrinkers; }
            set { Set(ref _nonAuthBeerDrinkers, value); }
        }
    }
}
