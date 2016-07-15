using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Famoser.BeerCompanion.Business.Enums;
using Famoser.BeerCompanion.Business.Models;
using Famoser.BeerCompanion.Business.Repository.Interfaces;
using Famoser.BeerCompanion.View.Enums;
using Famoser.FrameworkEssentials.Services.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using IProgressService = Famoser.BeerCompanion.View.Services.IProgressService;

namespace Famoser.BeerCompanion.View.ViewModels
{
    public class DrinkerCycleViewModel : ViewModelBase
    {
        private readonly IDrinkerCycleRepository _drinkerCycleRepository;
        private readonly IProgressService _progressService;
        private readonly IHistoryNavigationService _navigationService;

        public DrinkerCycleViewModel(IDrinkerCycleRepository drinkerCycleRepository, IProgressService progressService, IHistoryNavigationService navigationService)
        {
            _drinkerCycleRepository = drinkerCycleRepository;
            _progressService = progressService;
            _navigationService = navigationService;

            _authDrinker = new RelayCommand<Person>(AuthDrinker, CanAuthenticateDrinker);
            _removeDrinker = new RelayCommand<Person>(RemoveDrinker, CanRemoveDrinker);

            _leaveGroupCommand = new RelayCommand(LeaveGroup);

            if (IsInDesignMode)
            {
                DrinkerCycle = _drinkerCycleRepository.GetSampleCycles()[0];
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
        
        public ObservableCollection<Person> SortedPersons
        {
            get
            {
                var first = new ObservableCollection<Person>(DrinkerCycle.NonAuthBeerDrinkers.OrderBy(p => p.Name));
                foreach (var authBeerDrinker in DrinkerCycle.AuthBeerDrinkers.OrderBy(p => p.Name))
                {
                    first.Add(authBeerDrinker);
                }
                return first;
            }
        }

        #region Commands

        private readonly RelayCommand<Person> _authDrinker;
		public ICommand AuthDrinkerCommand { get { return _authDrinker; } }

        private bool CanAuthenticateDrinker(Person person)
        {
            var drink = person as Drinker;
            if (drink != null && drink.NonAuthDrinkerCycleGuids != null && drink.NonAuthDrinkerCycleGuids.Contains(DrinkerCycle.Guid))
                return true;
            return false;
        }

        private async void AuthDrinker(Person person)
        {
            _progressService.ShowProgress(ProgressKeys.InDebugMode);
            var drink = person as Drinker;
            if (drink != null && drink.NonAuthDrinkerCycleGuids.Contains(DrinkerCycle.Guid))
            {
                drink.NonAuthDrinkerCycleGuids.Remove(DrinkerCycle.Guid);
                drink.AuthDrinkerCycleGuids.Add(DrinkerCycle.Guid);
                DrinkerCycle.AuthBeerDrinkers.Add(drink);
                DrinkerCycle.NonAuthBeerDrinkers.Remove(drink);

                RaisePropertyChanged(() => SortedPersons);
                await _drinkerCycleRepository.AuthenticateUser(DrinkerCycle.Name, drink.Guid);
            }

            _progressService.HideProgress(ProgressKeys.InDebugMode);
        }

        private readonly RelayCommand<Person> _removeDrinker;
        public ICommand RemoveDrinkerCommand { get { return _removeDrinker; } }

        private bool CanRemoveDrinker(Person person)
        {
            var drink = person as Drinker;
            if (drink != null)
                return true;
            return false;
        }

        private async void RemoveDrinker(Person person)
        {
            _progressService.ShowProgress(ProgressKeys.InDebugMode);
            var drink = person as Drinker;
            if (drink != null)
            {
                if (DrinkerCycle.AuthBeerDrinkers.Contains(person))
                    DrinkerCycle.AuthBeerDrinkers.Remove(person);
                else if (DrinkerCycle.NonAuthBeerDrinkers.Contains(person))
                    DrinkerCycle.NonAuthBeerDrinkers.Remove(person);

                await _drinkerCycleRepository.RemoveUser(DrinkerCycle.Name, person.Guid);
                await SimpleIoc.Default.GetInstance<MainViewModel>().RefreshCycles();

                RaisePropertyChanged(() => SortedPersons);
                RaisePropertyChanged(() => DrinkerCycle);
            }

            _progressService.HideProgress(ProgressKeys.InDebugMode);
        }
        #endregion


        private readonly RelayCommand _leaveGroupCommand;
        public ICommand LeaveGroupCommand { get { return _leaveGroupCommand; } }

        private async void LeaveGroup()
        {
            _progressService.ShowProgress(ProgressKeys.RemovingSelfFromGroup);
            
            await _drinkerCycleRepository.RemoveSelf(DrinkerCycle.Name);
            await SimpleIoc.Default.GetInstance<MainViewModel>().RefreshCycles();
            _navigationService.GoBack();

            _progressService.HideProgress(ProgressKeys.RemovingSelfFromGroup);
        }
    }
}
