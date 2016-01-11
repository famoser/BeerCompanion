using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.BeerCompanion.View.Enums;
using GalaSoft.MvvmLight.Views;

namespace Famoser.BeerCompanion.Presentation.WinUniversal.Platform
{
    public static class NavigationHelper
    {
        public static INavigationService CreateNavigationService()
        {
            var navigationService = new NavigationService();

            navigationService.Configure(PageKeys.Main.ToString(), typeof(Pages.MainPage));
            navigationService.Configure(PageKeys.Wizard.ToString(), typeof(Pages.WizardPage));
            navigationService.Configure(PageKeys.DrinkerCycle.ToString(), typeof(Pages.DrinkerCyclePage));

            return navigationService;
        }
    }
}
