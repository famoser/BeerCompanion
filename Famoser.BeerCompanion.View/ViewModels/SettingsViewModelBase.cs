using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Business.Enums;
using Famoser.BeerCompanion.Business.Models;
using Famoser.BeerCompanion.Business.Repository.Interfaces;
using Famoser.BeerCompanion.Business.Services;
using Famoser.BeerCompanion.View.Models;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;

namespace Famoser.BeerCompanion.View.ViewModels
{
    public abstract class SettingsViewModelBase : ViewModelBase
    {
        private readonly ISettingsRepository _settingsRepository;
        private readonly IStorageService _storageService;

        public SettingsViewModelBase(ISettingsRepository settingsRepository, IStorageService storageService)
        {
            _settingsRepository = settingsRepository;
            _storageService = storageService;
            
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

        protected UserInformations UserInfo;

        private async void Initialize()
        {
            UserInfo = await _settingsRepository.GetUserInformations();
            var colorsJson = await _storageService.GetAssetFile(AssetFileKeys.ColorsJson);
            Colors = JsonConvert.DeserializeObject<ObservableCollection<Color>>(colorsJson);
            SelectedColor = Colors.FirstOrDefault(c => c.ColorValue == UserInfo.Color);
            if (SelectedColor == null)
            {
                var rand = new Random(DateTime.Now.Millisecond);
                SelectedColor = Colors[rand.Next(0, Colors.Count - 1)];
            }

            Name = UserInfo.Name;
        }


        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (Set(ref _name, value))
                    ValidateInput();
            }
        }

        private Color _selectedColor;
        public Color SelectedColor
        {
            get { return _selectedColor; }
            set
            {
                if (Set(ref _selectedColor, value))
                    ValidateInput();
            }
        }

        public abstract void ValidateInput();

        private ObservableCollection<Color> _colors;
        public ObservableCollection<Color> Colors
        {
            get { return _colors; }
            set { Set(ref _colors, value); }
        }

    }
}
