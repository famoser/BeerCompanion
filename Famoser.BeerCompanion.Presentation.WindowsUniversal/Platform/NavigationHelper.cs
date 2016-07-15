using Famoser.BeerCompanion.Presentation.WindowsUniversal.Pages;
using Famoser.BeerCompanion.View.Enums;
using Famoser.FrameworkEssentials.Services.Interfaces;
using Famoser.FrameworkEssentials.UniversalWindows.Platform;

namespace Famoser.BeerCompanion.Presentation.WindowsUniversal.Platform
{
    public class NavigationHelper
    {
        public static IHistoryNavigationService CreateNavigationService()
        {
            var navigationService = new HistoryNavigationService();

            navigationService.Configure(PageKeys.Main.ToString(), typeof(Pages.MainPage));
            navigationService.Configure(PageKeys.Wizard.ToString(), typeof(WizardPage));
            navigationService.Configure(PageKeys.DrinkerCycle.ToString(), typeof(DrinkerCyclePage));
            navigationService.Configure(PageKeys.Settings.ToString(), typeof(SettingsPage));

            return navigationService;
        }
    }
}
