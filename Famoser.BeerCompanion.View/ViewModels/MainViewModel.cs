using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Famoser.BeerCompanion.Business.Enums;
using Famoser.BeerCompanion.Business.Models;
using Famoser.BeerCompanion.Business.Repository.Interfaces;
using Famoser.BeerCompanion.Business.Services;
using Famoser.BeerCompanion.View.Enums;
using Famoser.BeerCompanion.View.Services;
using Famoser.BeerCompanion.View.Utils;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;

namespace Famoser.BeerCompanion.View.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IBeerRepository _beerRepository;
        private readonly ISettingsRepository _settingsRepository;
        private readonly IDrinkerCycleRepository _drinkerCycleRepository;
        private readonly IInteractionService _interactionService;
        private readonly INavigationService _navigationService;
        private readonly IProgressService _progressService;

        public MainViewModel(ISettingsRepository settingsRepository, IBeerRepository beerRepository, IInteractionService interactionService, IDrinkerCycleRepository drinkerCycleRepository, INavigationService navigationService, IProgressService progressService)
        {
            _settingsRepository = settingsRepository;
            _beerRepository = beerRepository;
            _interactionService = interactionService;
            _drinkerCycleRepository = drinkerCycleRepository;
            _navigationService = navigationService;
            _progressService = progressService;

            _addBeer = new RelayCommand(AddBeer);
            _removeBeer = new RelayCommand(RemoveBeer, () => CanRemoveBeer);
            _addGroup = new RelayCommand(AddGroup, () => CanAddGroup);

            _refreshCommand = new RelayCommand(Refresh, () => CanRefresh);
            _openSettingsCommand = new RelayCommand(OpenSettings);

            Messenger.Default.Register<Messages>(this, EvaluateMessages);

            if (IsInDesignMode)
            {
                DrinkerCycles = _drinkerCycleRepository.GetSampleCycles();
                UserInformation = settingsRepository.GetSampleUserInformations();
            }
            else
            {
                Initialize();
            }
        }

        private void EvaluateMessages(Messages obj)
        {
            if (obj == Messages.WizardExited)
                Initialize();
        }

        //enable for debug purposes
        private bool _doReset = false;
        private bool _isInitializing;
        private async void Initialize()
        {
            _progressService.ShowProgress(ProgressKeys.InitializingApplication);
            _isInitializing = true;
            _refreshCommand.RaiseCanExecuteChanged();

            if (_doReset)
            {
                await ResetHelper.Instance.ResetApplication();
                _doReset = false;
            }

            DrinkerCycles = await _drinkerCycleRepository.GetSavedCycles();
            UserInformation = await _settingsRepository.GetUserInformations();

            if (!UserInformation.FirstTime)
            {
                if (await _interactionService.CanUseInternet())
                {
                    await SaveBeers();
                    await RefreshCycles();
                }
            }
            else
            {
                _navigationService.NavigateTo(PageKeys.Wizard.ToString());
            }

            _isInitializing = false;
            _refreshCommand.RaiseCanExecuteChanged();
            _progressService.HideProgress(ProgressKeys.InitializingApplication);
        }

        public async Task RefreshCycles()
        {
            _progressService.ShowProgress(ProgressKeys.LoadingCycles);
            if (await _interactionService.CanUseInternet())
            {
                var newCylces = await _drinkerCycleRepository.AktualizeCycles();
                if (newCylces != null)
                {
                    DrinkerCycles = newCylces;
                    RaisePropertyChanged(() => DrinkerCycles);

                    await _drinkerCycleRepository.SaveCycles(DrinkerCycles);
                }
            }
            _progressService.HideProgress(ProgressKeys.LoadingCycles);
        }

        private async Task SaveBeers()
        {
            _progressService.ShowProgress(ProgressKeys.LoadingBeers);
            if (await _interactionService.CanUseInternet())
            {
                var beers = await _beerRepository.SyncBeers(UserInformation.Beers, UserInformation.Guid);
                if (beers != null)
                {
                    UserInformation.Beers = beers;
                    RaisePropertyChanged(() => SortedBeers);
                    RaisePropertyChanged(() => DrinkerCycles);
                }
            }
            await _settingsRepository.SaveUserInformations(UserInformation);
            _progressService.HideProgress(ProgressKeys.LoadingBeers);
        }

        private ObservableCollection<DrinkerCycle> _drinkerCycles;
        public ObservableCollection<DrinkerCycle> DrinkerCycles
        {
            get { return _drinkerCycles; }
            set { Set(ref _drinkerCycles, value); }
        }

        private UserInformations _userInformation;
        public UserInformations UserInformation
        {
            get { return _userInformation; }
            set
            {
                if (Set(ref _userInformation, value))
                    _removeBeer.RaiseCanExecuteChanged();
            }
        }
        
		public ObservableCollection<Beer> SortedBeers { get { return UserInformation != null  && UserInformation.Beers != null ? new ObservableCollection<Beer> (UserInformation.Beers.Where (b => !b.DeletePending).OrderByDescending (b => b.DrinkTime)) : new ObservableCollection<Beer> (); } }

        #region Commands
        private readonly RelayCommand _addBeer;
        public ICommand AddBeerCommand { get { return _addBeer; } }

        private async void AddBeer()
        {
            UserInformation.Beers.Add(new Beer()
            {
                DrinkTime = DateTime.Now,
                Guid = Guid.NewGuid()
            });
            _removeBeer.RaiseCanExecuteChanged();

            await SaveBeers();
        }

        private readonly RelayCommand _removeBeer;
		public ICommand RemoveBeerCommand { get { return _removeBeer; } }

		public bool CanRemoveBeer 
		{ 
			get 
			{ 
				if (UserInformation != null && UserInformation.Beers != null) {
					return UserInformation.Beers.Any (b => !b.DeletePending);
				}
				return false;
			} 
		}

        private async void RemoveBeer()
        {
            var beer = UserInformation.Beers.Where(b => !b.DeletePending).OrderByDescending(b => b.DrinkTime).FirstOrDefault();
            if (beer != null)
            {
                beer.DeletePending = true;
                _removeBeer.RaiseCanExecuteChanged();

                await SaveBeers();
            }
        }

        private readonly RelayCommand _refreshCommand;
        public ICommand RefreshCommand { get { return _refreshCommand; } }

        public bool CanRefresh { get { return !_isInitializing; } }

        private void Refresh()
        {
            Initialize();
        }

        private readonly RelayCommand _openSettingsCommand;
        public ICommand OpenSettingsCommand { get { return _openSettingsCommand; } }

        private void OpenSettings()
        {
            _navigationService.NavigateTo(PageKeys.Settings.ToString());
        }

        private string _newGroupName;
        public string NewGroupName
        {
            get { return _newGroupName; }
            set
            {
                if (Set(ref _newGroupName, value))
                    GetStatusForGroupName();
            }
        }

        private string _newGroupNameMessage;
        public string NewGroupNameMessage
        {
            get { return _newGroupNameMessage; }
            set
            {
                if (Set(ref _newGroupNameMessage, value))
                    _addGroup.RaiseCanExecuteChanged();
            }
        }

        private Guid _lastProcess;
        private async void GetStatusForGroupName()
        {
            var g = Guid.NewGuid();
            _lastProcess = g;
            if (string.IsNullOrEmpty(NewGroupName))
            {
                NewGroupNameMessage = "";
                _canAddNewGroupBlock = true;
            }
            else if (DrinkerCycles.Any(d => d.Name == NewGroupName))
            {
                if (_lastProcess == g)
                {
                    _canAddNewGroupBlock = true;
                    NewGroupNameMessage = "Du bist bereits in dieser Gruppe";
                }
            }
            else
            {
                var res = await _drinkerCycleRepository.DoesExists(NewGroupName);
                if (_lastProcess == g)
                {
                    _canAddNewGroupBlock = false;
                    NewGroupNameMessage = res
                        ? "Du wirst dieser Gruppe hinzugefügt"
                        : "Diese Gruppe existiert noch nicht, und wird neu erstellt";
                }
            }
        }

		private readonly RelayCommand _addGroup;
		public ICommand AddGroupCommand { get { return _addGroup; } }

        private bool _canAddNewGroupBlock;
		private bool CanAddGroup { get { return !string.IsNullOrEmpty (NewGroupName) && !_isAddingGroup && !_canAddNewGroupBlock; } }

        private bool _isAddingGroup;
        private async void AddGroup()
        {
            _isAddingGroup = true;
            _addGroup.RaiseCanExecuteChanged();

            await _drinkerCycleRepository.AddSelf(NewGroupName);
            await RefreshCycles();

            NewGroupName = "";
            _isAddingGroup = false;
            _addGroup.RaiseCanExecuteChanged();
        }
        #endregion

        public void NavigateToCycle(DrinkerCycle cycle)
        {
            if (cycle.IsAuthenticated)
            {
                _navigationService.NavigateTo(PageKeys.DrinkerCycle.ToString());
                Messenger.Default.Send(cycle, Messages.Select);
            }
        }
    }
}
