using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.BeerCompanion.View.Enums;
using GalaSoft.MvvmLight.Views;

namespace Famoser.BeerCompanion.Presentation.WinUniversal.Platform
{
    public class NavigationHelper
    {
        public static INavigationService CreateNavigationService()
        {
            var navigationService = new CustomNavigationService();

            navigationService.Implementation.Configure(PageKeys.Main.ToString(), typeof(Pages.MainPage));
            navigationService.Implementation.Configure(PageKeys.Wizard.ToString(), typeof(Pages.WizardPage));
            navigationService.Implementation.Configure(PageKeys.DrinkerCycle.ToString(), typeof(Pages.DrinkerCyclePage));
            navigationService.Implementation.Configure(PageKeys.Settings.ToString(), typeof(Pages.SettingsPage));

            return navigationService;
        }
    }
}
