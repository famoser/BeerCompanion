using Famoser.BeerCompanion.Business.Services;
using Famoser.BeerCompanion.Presentation.WindowsUniversal.Platform;
using Famoser.BeerCompanion.View.ViewModels;
using Famoser.FrameworkEssentials.Services.Interfaces;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using IStorageService = Famoser.BeerCompanion.Business.Services.IStorageService;

namespace Famoser.BeerCompanion.Presentation.WindowsUniversal.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public partial class ViewModelLocator : BaseViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Create design time view services and models
            SimpleIoc.Default.Register<IStorageService, StorageService>();
            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<IInteractionService, InteractionService>();

            var navigationService = NavigationHelper.CreateNavigationService();
            SimpleIoc.Default.Register<IHistoryNavigationService>(() => navigationService);
        }
    }
}
