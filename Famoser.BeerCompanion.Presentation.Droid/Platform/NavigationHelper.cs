using System;
using Famoser.BeerCompanion.Presentation.Droid.Activities;
using Famoser.BeerCompanion.View.Enums;
using GalaSoft.MvvmLight.Views;

namespace Famoser.BeerCompanion.Presentation.Droid
{
	public class NavigationHelper
    {
        public static INavigationService CreateNavigationService()
        {
            var navigationService = new NavigationService();

            navigationService.Configure(PageKeys.Main.ToString(), typeof(MainActivity));
            navigationService.Configure(PageKeys.Wizard.ToString(), typeof(WizardActivity));
            navigationService.Configure(PageKeys.DrinkerCycle.ToString(), typeof(DrinkerActivity));

            return navigationService;
        }
	}
}

