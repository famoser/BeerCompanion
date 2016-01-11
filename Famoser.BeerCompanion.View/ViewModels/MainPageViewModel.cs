using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Famoser.BeerCompanion.Business.Enums;
using Famoser.BeerCompanion.Business.Models;
using Famoser.BeerCompanion.Business.Repository.Interfaces;
using Famoser.BeerCompanion.Business.Services;
using Famoser.BeerCompanion.View.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace Famoser.BeerCompanion.View.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private IBeerRepository _beerRepository;
        private ISettingsRepository _settingsRepository;
        private IDrinkerCycleRepository _drinkerCycleRepository;
        private IInteractionService _interactionService;

        public MainPageViewModel(ISettingsRepository settingsRepository, IBeerRepository beerRepository, IInteractionService interactionService, IDrinkerCycleRepository drinkerCycleRepository)
        {
            _settingsRepository = settingsRepository;
            _beerRepository = beerRepository;
            _interactionService = interactionService;
            _drinkerCycleRepository = drinkerCycleRepository;

            _addBeer = new RelayCommand(AddBeer);
            _removeBeer = new RelayCommand(RemoveBeer, () => CanRemoveBeer);

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

        private async void Initialize()
        {
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

                    var newCylces = await _drinkerCycleRepository.AktualizeCycles();
                    if (newCylces != null)
                    {
                        DrinkerCycle = newCylces;
                        await _drinkerCycleRepository.SaveCycles(DrinkerCycle);
                    }
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
        #endregion
    }
}
