using System.Collections.ObjectModel;
using System.Linq;
using Famoser.BeerCompanion.Business.Models;
using Famoser.BeerCompanion.Business.Repository.Interfaces;
using Famoser.BeerCompanion.Common.Services;
using GalaSoft.MvvmLight;

namespace Famoser.BeerCompanion.View.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private IBeerRepository _beerRepository;
        private ISettingsRepository _settingsRepository;
        private IPermissionService _permissionService;

        public MainPageViewModel(ISettingsRepository settingsRepository, IBeerRepository beerRepository, IPermissionService permissionService)
        {
            _settingsRepository = settingsRepository;
            _beerRepository = beerRepository;
            _permissionService = permissionService;

            if (IsInDesignMode)
            {
                DrinkerCycle = beerRepository.GetSampleCycles();
                Beers = settingsRepository.GetSampleUserInformations().Beers;
            }
            else
            {
                Initialize();
            }
        }

        private async void Initialize()
        {
            DrinkerCycle = await _beerRepository.GetSavedCycles();
            var usrInfo = (await _settingsRepository.GetUserInformations());
            usrInfo.Beers = new ObservableCollection<Beer>(usrInfo.Beers.Where(b => !b.DeletePending));
            UserInformation = usrInfo;

            if (await _permissionService.CanUseInternet())
            {
                var beers = await _beerRepository.SyncBeers();
                if (beers != null)
                {
                    UserInformation.Beers = beers;
                    await _settingsRepository.SaveUserInformations(UserInformation);
                }

                var newCylces = await _beerRepository.AktualizeCycles();
                if (newCylces != null)
                {
                    DrinkerCycle = newCylces;
                    await _beerRepository.SaveCycles(DrinkerCycle);
                }
            }
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
            set { Set(ref _userInformation, value); }
        }
    }
}
