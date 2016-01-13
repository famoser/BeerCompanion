using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Famoser.BeerCompanion.Business.Models;
using Famoser.BeerCompanion.Business.Repository.Interfaces;
using Famoser.BeerCompanion.View.Enums;
using Famoser.BeerCompanion.View.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace Famoser.BeerCompanion.View.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly ISettingsRepository _settingsRepository;
        private readonly IProgressService _progressService;
        private readonly INavigationService _navigationService;

        public SettingsViewModel(ISettingsRepository settingsRepository, IProgressService progressService, INavigationService navigationService)
        {
            _settingsRepository = settingsRepository;
            _progressService = progressService;
            _navigationService = navigationService;

            _saveSettings = new RelayCommand(SaveSettings, () => CanSaveSettings);

            if (IsInDesignMode)
                UserInformation = _settingsRepository.GetSampleUserInformations();
            else
                Initialize();
        }

        private async void Initialize()
        {
            _progressService.ShowProgress(ProgressKeys.LoadingSettings);

            UserInformation = await _settingsRepository.GetUserInformations();

            _progressService.HideProgress(ProgressKeys.LoadingSettings);
        }

        private UserInformations _userInformation;
        public UserInformations UserInformation
        {
            get { return _userInformation; }
            set
            {
                if (Set(ref _userInformation, value))
                    _saveSettings.RaiseCanExecuteChanged();
            }
        }

        private readonly RelayCommand _saveSettings;
        public ICommand SaveSettingsCommand { get { return _saveSettings; } }

        public bool CanSaveSettings { get { return !_isSaving && UserInformation != null; } }

        private bool _isSaving;
        private async void SaveSettings()
        {
            _progressService.ShowProgress(ProgressKeys.LoadingSettings);
            _isSaving = true;
            _saveSettings.RaiseCanExecuteChanged();

            await _settingsRepository.SaveUserInformations(UserInformation);
            
            _progressService.HideProgress(ProgressKeys.LoadingSettings);
            _isSaving = true;
            _saveSettings.RaiseCanExecuteChanged();

            _navigationService.GoBack();
        }
    }
}
