using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using GalaSoft.MvvmLight;

namespace Famoser.BeerCompanion.Business.Models
{
    [DataContract]
    public class DrinkerCycle : ObservableObject
    {
        public DrinkerCycle()
        {
            AuthBeerDrinkers = new ObservableCollection<Person>();
            NonAuthBeerDrinkers = new ObservableCollection<Person>();
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

        private bool _isAuthenticated;
        [DataMember]
        public bool IsAuthenticated
        {
            get { return _isAuthenticated; }
            set { Set(ref _isAuthenticated, value); }
        }

        private ObservableCollection<Person> _authBeerDrinkers;
        public ObservableCollection<Person> AuthBeerDrinkers
        {
            get { return _authBeerDrinkers; }
            set { Set(ref _authBeerDrinkers, value); }
        }

        private ObservableCollection<Person> _nonAuthBeerDrinkers;
        public ObservableCollection<Person> NonAuthBeerDrinkers
        {
            get { return _nonAuthBeerDrinkers; }
            set { Set(ref _nonAuthBeerDrinkers, value); }
        }

        //smart properties
        public int GetTotalBeers => AuthBeerDrinkers.Sum(a => a.GetTotalBeers);

        public int GetTotalPersons => AuthBeerDrinkers.Count;

        public double GetBeersPerPerson => (double)GetTotalBeers / GetTotalPersons;

        public Person GetLastDrinker => AuthBeerDrinkers.OrderByDescending(a => a.GetLastBeer ?? DateTime.MinValue).FirstOrDefault();
    }
}
