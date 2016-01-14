using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Famoser.BeerCompanion.Business.Models;
using Famoser.BeerCompanion.Business.Repository.Interfaces;
using Famoser.BeerCompanion.Business.Services;
using Famoser.BeerCompanion.View.Enums;
using Famoser.BeerCompanion.View.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace Famoser.BeerCompanion.View.ViewModels
{
    public class SettingsViewModel : SettingsViewModelBase
    {
        private readonly ISettingsRepository _settingsRepository;
        private readonly IProgressService _progressService;
        private readonly INavigationService _navigationService;

        public SettingsViewModel(ISettingsRepository settingsRepository, IProgressService progressService, INavigationService navigationService, IStorageService storageService) :
            base(settingsRepository, storageService)
        {
            _settingsRepository = settingsRepository;
            _progressService = progressService;
            _navigationService = navigationService;

            _saveSettings = new RelayCommand(SaveSettings, () => CanSaveSettings);
        }

        private readonly RelayCommand _saveSettings;
        public ICommand SaveSettingsCommand { get { return _saveSettings; } }

        public bool CanSaveSettings { get { return !_isSaving && !string.IsNullOrEmpty(Name) && SelectedColor != null; } }

        private bool _isSaving;
        private async void SaveSettings()
        {
            _progressService.ShowProgress(ProgressKeys.LoadingSettings);
            _isSaving = true;
            _saveSettings.RaiseCanExecuteChanged();

            UserInfo.Name = Name;
            UserInfo.Color = SelectedColor.ColorValue;

            await _settingsRepository.SaveUserInformations(UserInfo);

            _progressService.HideProgress(ProgressKeys.LoadingSettings);
            _isSaving = false;
            _saveSettings.RaiseCanExecuteChanged();

            _navigationService.GoBack();
        }

        public override void ValidateInput()
        {
            if (_saveSettings != null)
                _saveSettings.RaiseCanExecuteChanged();
        }
    }
}
