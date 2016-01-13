using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Business.Enums;
using Famoser.BeerCompanion.Business.Models;
using Famoser.BeerCompanion.Business.Repository.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace Famoser.BeerCompanion.View.ViewModels
{
    public class DrinkerCycleViewModel : ViewModelBase
    {
        private readonly IDrinkerCycleRepository _drinkerCycleRepository;

        public DrinkerCycleViewModel(IDrinkerCycleRepository drinkerCycleRepository)
        {
            _drinkerCycleRepository = drinkerCycleRepository;
            if (IsInDesignMode)
            {
                DrinkerCycle = _drinkerCycleRepository.GetSampleCycles()[1];
            }
            Messenger.Default.Register<DrinkerCycle>(this, Messages.Select, SelectDrinkerCycle);
        }

        private void SelectDrinkerCycle(DrinkerCycle obj)
        {
            DrinkerCycle = obj;
        }

        private DrinkerCycle _drinkerCycles;
        public DrinkerCycle DrinkerCycle
        {
            get { return _drinkerCycles; }
            set { Set(ref _drinkerCycles, value); }
        }

        
        public int GetTotalBeers => DrinkerCycle.AuthBeerDrinkers.Sum(a => a.GetTotalBeers);

        public int GetTotalPersons => DrinkerCycle.AuthBeerDrinkers.Count;

        public double GetBeersPerPerson => (double)GetTotalBeers / GetTotalPersons;

        public Person GetLastDrinker => DrinkerCycle.AuthBeerDrinkers.OrderByDescending(a => a.GetLastBeer ?? DateTime.MinValue).FirstOrDefault();
    }
}
