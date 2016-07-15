using Famoser.BeerCompanion.View.Enums;
using Famoser.FrameworkEssentials.Services.Interfaces;
using Famoser.FrameworkEssentials.UniversalWindows.Platform;
using GalaSoft.MvvmLight.Views;

namespace Famoser.BeerCompanion.Presentation.WinUniversal.Platform
{
    public class NavigationHelper
    {
        public static IHistoryNavigationService CreateNavigationService()
        {
            var navigationService = new HistoryNavigationService();

            navigationService.Configure(PageKeys.Main.ToString(), typeof(Pages.MainPage));
            navigationService.Configure(PageKeys.Wizard.ToString(), typeof(Pages.WizardPage));
            navigationService.Configure(PageKeys.DrinkerCycle.ToString(), typeof(Pages.DrinkerCyclePage));
            navigationService.Configure(PageKeys.Settings.ToString(), typeof(Pages.SettingsPage));

            return navigationService;
        }
    }
}
