using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using GalaSoft.MvvmLight;

namespace Famoser.BeerCompanion.Business.Models
{
    [DataContract]
    public abstract class Person : ObservableObject
    {
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

        private string _color;
        [DataMember]
        public string Color
        {
            get { return _color; }
            set { Set(ref _color, value); }
        }

        private ObservableCollection<DrinkerCycle> _authDrinkerCycles;
        public ObservableCollection<DrinkerCycle> AuthDrinkerCycles
        {
            get { return _authDrinkerCycles; }
            set { Set(ref _authDrinkerCycles, value); }
        }

        private ObservableCollection<DrinkerCycle> _nonAuthDrinkerCycles;
        public ObservableCollection<DrinkerCycle> NonAuthDrinkerCycles
        {
            get { return _nonAuthDrinkerCycles; }
            set { Set(ref _nonAuthDrinkerCycles, value); }
        }

        public abstract int GetTotalBeers { get; }
        public abstract DateTime? GetLastBeer { get; }
    }
}
