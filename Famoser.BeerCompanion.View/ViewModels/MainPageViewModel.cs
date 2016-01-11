using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Famoser.BeerCompanion.Business.Enums;
using Famoser.BeerCompanion.Business.Models;
using Famoser.BeerCompanion.Business.Repository.Interfaces;
using Famoser.BeerCompanion.Business.Services;
using Famoser.BeerCompanion.View.Enums;
using Famoser.BeerCompanion.View.Models;
using Famoser.BeerCompanion.View.Utils;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;

namespace Famoser.BeerCompanion.View.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private IBeerRepository _beerRepository;
        private ISettingsRepository _settingsRepository;
        private IDrinkerCycleRepository _drinkerCycleRepository;
        private IInteractionService _interactionService;
        private INavigationService _navigationService;

        public MainPageViewModel(ISettingsRepository settingsRepository, IBeerRepository beerRepository, IInteractionService interactionService, IDrinkerCycleRepository drinkerCycleRepository, INavigationService navigationService)
        {
            _settingsRepository = settingsRepository;
            _beerRepository = beerRepository;
            _interactionService = interactionService;
            _drinkerCycleRepository = drinkerCycleRepository;
            _navigationService = navigationService;

            _addBeer = new RelayCommand(AddBeer);
            _removeBeer = new RelayCommand(RemoveBeer, () => CanRemoveBeer);
            _addGroup = new RelayCommand(AddGroup, () => CanAddGroup);

            Messenger.Default.Register<Messages>(this, EvaluateMessages);

            if (IsInDesignMode)
            {
                DrinkerCycle = _drinkerCycleRepository.GetSampleCycles();
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

        private bool _doReset = false;
        private async void Initialize()
        {
            if (_doReset)
            {
                await ResetHelper.Instance.ResetApplication();
                _doReset = false;
            }

            DrinkerCycle = await _drinkerCycleRepository.GetSavedCycles();
            UserInformation = await _settingsRepository.GetUserInformations();

            if (!UserInformation.FirstTime)
            {
                if (await _interactionService.CanUseInternet())
                {
                    var beers = await _beerRepository.SyncBeers(UserInformation.Beers, UserInformation.Guid);
                    if (beers != null)
                    {
                        UserInformation.Beers = beers;
                        await _settingsRepository.SaveUserInformations(UserInformation);
                    }

                    await RefreshCycles();
                }
            }
        }

        private async Task RefreshCycles()
        {
            if (await _interactionService.CanUseInternet())
            {
                var newCylces = await _drinkerCycleRepository.AktualizeCycles();
                if (newCylces != null)
                {
                    DrinkerCycle = newCylces;
                    await _drinkerCycleRepository.SaveCycles(DrinkerCycle);
                }
            }
        }

        private async Task SaveBeers()
        {
            if (await _interactionService.CanUseInternet())
            {
                var beers = await _beerRepository.SyncBeers(UserInformation.Beers, UserInformation.Guid);
                if (beers != null)
                {
                    UserInformation.Beers = beers;
                }
            }
            await _settingsRepository.SaveUserInformations(UserInformation);
        }

        private ObservableCollection<DrinkerCycle> _drinkerCycles;
        public ObservableCollection<DrinkerCycle> DrinkerCycle
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

        #region Commands
        private readonly RelayCommand _addBeer;
        public ICommand AddBeerCommand => _addBeer;

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
        public ICommand RemoveBeerCommand => _removeBeer;

        private bool CanRemoveBeer => UserInformation?.Beers != null && UserInformation.Beers.Any(b => !b.DeletePending);

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
            else if (DrinkerCycle.Any(d => d.Name == NewGroupName))
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
        public ICommand AddGroupCommand => _addGroup;

        private bool _canAddNewGroupBlock;
        private bool CanAddGroup => !string.IsNullOrEmpty(NewGroupName) && !_isAddingGroup && !_canAddNewGroupBlock;

        private bool _isAddingGroup;
        private async void AddGroup()
        {
            _isAddingGroup = true;
            _addGroup.RaiseCanExecuteChanged();

            await _drinkerCycleRepository.AddSelf(NewGroupName, UserInformation.Guid);
            await RefreshCycles();

            NewGroupName = "";
            _isAddingGroup = false;
            _addGroup.RaiseCanExecuteChanged();
        }
        #endregion

        public void NavigateToCycle(DrinkerCycle cycle)
        {
            _navigationService.NavigateTo(PageKeys.DrinkerCycle.ToString());
            Messenger.Default.Send(cycle, Messages.Select);
        }
    }
}
