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
using Famoser.BeerCompanion.View.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;

namespace Famoser.BeerCompanion.View.ViewModels
{
    public class WizardViewModel : ViewModelBase
    {
        private readonly IInteractionService _interactionService;
        private readonly ISettingsRepository _settingsRepository;
        private readonly IStorageService _storageService;

        public WizardViewModel(IInteractionService interactionService, ISettingsRepository settingsRepository, IStorageService storageService)
        {
            _interactionService = interactionService;
            _settingsRepository = settingsRepository;
            _storageService = storageService;

            _exitWizard = new RelayCommand(ExitWizard, () => CanExitWizard);

            if (!IsInDesignMode)
                Initialize();
            else
            {
                Name = "DesignName";
                Colors = new ObservableCollection<Color>()
                {
                    new Color("F44336", "Red"),
                    new Color("E91E63", "Pink"),
                    new Color("9C27B0", "Purple"),
                    new Color("673AB7", "Deep Purple"),
                    new Color("F44336", "Red"),
                    new Color("E91E63", "Pink"),
                    new Color("9C27B0", "Purple"),
                    new Color("673AB7", "Deep Purple"),
                    new Color("F44336", "Red"),
                    new Color("E91E63", "Pink"),
                    new Color("9C27B0", "Purple"),
                    new Color("673AB7", "Deep Purple")
                };
                SelectedColor = Colors[2];
            }
        }

        private async void Initialize()
        {
            var usrInfo = await _settingsRepository.GetUserInformations();
            var colorsJson = await _storageService.GetAssetFile(AssetFileKeys.ColorsJson);
            Colors = JsonConvert.DeserializeObject<ObservableCollection<Color>>(colorsJson);

            if (usrInfo.FirstTime)
                usrInfo.Name = await _interactionService.GetPersonalName();

            Name = usrInfo.Name;
            SelectedColor = Colors.FirstOrDefault(c => c.ColorValue == usrInfo.Color);
            if (SelectedColor == null)
            {
                var rand = new Random(DateTime.Now.Millisecond);
                SelectedColor = Colors[rand.Next(0, Colors.Count - 1)];
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (Set(ref _name, value))
                    _exitWizard.RaiseCanExecuteChanged();
            }
        }

        private Color _selectedColor;
        public Color SelectedColor
        {
            get { return _selectedColor; }
            set
            {
                if (Set(ref _selectedColor, value))
                    _exitWizard.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<Color> _colors;
        public ObservableCollection<Color> Colors
        {
            get { return _colors; }
            set { Set(ref _colors, value); }
        }

		private readonly RelayCommand _exitWizard;
		public ICommand ExitWizardCommand { get { return _exitWizard; } }

		private bool CanExitWizard { get { return SelectedColor != null && !string.IsNullOrEmpty (Name) && !_isExiting; } }

        private bool _isExiting;
        private async void ExitWizard()
        {
            _isExiting = true;
            var usrInfo = await _settingsRepository.GetUserInformations();
            usrInfo.Color = SelectedColor.ColorValue;
            usrInfo.Name = Name;
            await _settingsRepository.SaveUserInformations(usrInfo);
            await _settingsRepository.SyncUserInformations(usrInfo);
            Messenger.Default.Send(Messages.WizardExited);
        }
    }
}
