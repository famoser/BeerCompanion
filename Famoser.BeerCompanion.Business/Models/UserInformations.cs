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

        public override int GetTotalBeers => Beers.Count;
        public override DateTime? GetLastBeer => Beers.LastOrDefault()?.DrinkTime;

        private ObservableCollection<Beer> _beers;
        public ObservableCollection<Beer> Beers
        {
            get { return _beers; }
            set
            {
                var oldbeer = _beers;
                if (Set(ref _beers, value))
                {
                    RaisePropertyChanged(() => SortedBeers);

                    if (_beers != null)
                        _beers.CollectionChanged += BeersOnCollectionChanged;

                    if (oldbeer != null)
                        oldbeer.CollectionChanged -= BeersOnCollectionChanged;
                }
            }
        }
        
        public ObservableCollection<Beer> SortedBeers => new ObservableCollection<Beer>(Beers.Where(b => !b.DeletePending).OrderByDescending(b => b.DrinkTime));

        private void BeersOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            RaisePropertyChanged(() => GetTotalBeers);
            RaisePropertyChanged(() => GetLastBeer);
            RaisePropertyChanged(() => SortedBeers);
        }
    }
}
