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
        private IDrinkerCycleRepository _drinkerCycleRepository;
        private IPermissionService _permissionService;

        public MainPageViewModel(ISettingsRepository settingsRepository, IBeerRepository beerRepository, IPermissionService permissionService, IDrinkerCycleRepository drinkerCycleRepository)
        {
            _settingsRepository = settingsRepository;
            _beerRepository = beerRepository;
            _permissionService = permissionService;
            _drinkerCycleRepository = drinkerCycleRepository;

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

        private async void Initialize()
        {
            DrinkerCycle = await _drinkerCycleRepository.GetSavedCycles();
            var usrInfo = (await _settingsRepository.GetUserInformations());
            usrInfo.Beers = new ObservableCollection<Beer>(usrInfo.Beers.Where(b => !b.DeletePending));
            UserInformation = usrInfo;

            if (await _permissionService.CanUseInternet())
            {
                var beers = await _beerRepository.SyncBeers();
                if (beers != null)
                {
                    UserInformation.Beers = beers;
                    await _settingsRepository.SaveUserInformations(UserInformation, true);
                }

                var newCylces = await _drinkerCycleRepository.AktualizeCycles();
                if (newCylces != null)
                {
                    DrinkerCycle = newCylces;
                    await _drinkerCycleRepository.SaveCycles(DrinkerCycle);
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
