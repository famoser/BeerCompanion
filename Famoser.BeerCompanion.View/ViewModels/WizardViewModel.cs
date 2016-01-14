using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Famoser.BeerCompanion.Business.Enums;
using Famoser.BeerCompanion.Business.Models;
using Famoser.BeerCompanion.Business.Repository.Interfaces;
using Famoser.BeerCompanion.Business.Services;
using Famoser.BeerCompanion.View.Enums;
using Famoser.BeerCompanion.View.Models;
using Famoser.BeerCompanion.View.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Newtonsoft.Json;

namespace Famoser.BeerCompanion.View.ViewModels
{
    public class WizardViewModel : SettingsViewModelBase
    {
        private readonly IInteractionService _interactionService;
        private readonly ISettingsRepository _settingsRepository;
        private readonly INavigationService _navigationService;
        private readonly IProgressService _progressService;

        public WizardViewModel(IInteractionService interactionService, ISettingsRepository settingsRepository, IStorageService storageService, INavigationService navigationService, IProgressService progressService) : 
            base(settingsRepository,storageService)
        {
            _interactionService = interactionService;
            _settingsRepository = settingsRepository;
            _navigationService = navigationService;
            _progressService = progressService;

            _exitWizard = new RelayCommand(ExitWizard, () => CanExitWizard);

            if (!IsInDesignMode)
                InitializeWizard();
        }

        private async void InitializeWizard()
        {
            if (UserInfo.FirstTime)
                UserInfo.Name = await _interactionService.GetPersonalName();
        }

		private readonly RelayCommand _exitWizard;
		public ICommand ExitWizardCommand { get { return _exitWizard; } }

		private bool CanExitWizard { get { return SelectedColor != null && !string.IsNullOrEmpty (Name) && !_isExiting; } }

        private bool _isExiting;
        private async void ExitWizard()
        {
            _isExiting = true;
            _exitWizard.RaiseCanExecuteChanged();
            _progressService.ShowProgress(ProgressKeys.ExitingWizard);

            UserInfo.Color = SelectedColor.ColorValue;
            UserInfo.Name = Name;
            await _settingsRepository.SaveUserInformations(UserInfo);
            await _settingsRepository.SyncUserInformations(UserInfo);

            Messenger.Default.Send(Messages.WizardExited);

            _isExiting = false;
            _exitWizard.RaiseCanExecuteChanged();
            _progressService.HideProgress(ProgressKeys.ExitingWizard);

            _navigationService.GoBack();
        }

        public override void ValidateInput()
        {
            if (_exitWizard != null)
                _exitWizard.RaiseCanExecuteChanged();
        }
    }
}
