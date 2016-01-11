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
    public class UserInformations : ObservableObject
    {
        public UserInformations()
        {
            Beers = new ObservableCollection<Beer>();
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

        private string _color;
        [DataMember]
        public string Color
        {
            get { return _color; }
            set { Set(ref _color, value); }
        }

        private bool _firstName;
        public bool FirstTime
        {
            get { return _firstName; }
            set { Set(ref _firstName, value); }
        }

        public int TotalBeers => Beers.Count;
        public DateTime? LastBeer => Beers.LastOrDefault()?.DrinkTime;

        private ObservableCollection<Beer> _beers;
        public ObservableCollection<Beer> Beers
        {
            get { return _beers; }
            set
            {
                var oldbeer = _beers;
                if (Set(ref _beers, value))
                {
                    if (_beers != null)
                        _beers.CollectionChanged += BeersOnCollectionChanged;

                    if (oldbeer != null)
                        oldbeer.CollectionChanged -= BeersOnCollectionChanged;
                }
            }
        }

        private void BeersOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            RaisePropertyChanged(() => TotalBeers);
            RaisePropertyChanged(() => LastBeer);
        }
    }
}
