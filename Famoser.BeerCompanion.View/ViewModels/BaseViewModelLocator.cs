using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Business.Repository;
using Famoser.BeerCompanion.Business.Repository.Interfaces;
using Famoser.BeerCompanion.Data;
using Famoser.BeerCompanion.Data.Services;
using Famoser.BeerCompanion.View.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace Famoser.BeerCompanion.View.ViewModels
{
    public class BaseViewModelLocator
    {
        public BaseViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IBeerRepository, BeerRepository>();
            SimpleIoc.Default.Register<ISettingsRepository, SettingsRepository>();
            SimpleIoc.Default.Register<IDrinkerCycleRepository, DrinkerCycleRepository>();
            
            SimpleIoc.Default.Register<IDataService, DataService>();
            SimpleIoc.Default.Register<IProgressService, ProgressService>();

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
            }
            else
            {
                // Create run time view services and models
            }
            SimpleIoc.Default.Register<ProgressViewModel>();

            SimpleIoc.Default.Register<MainPageViewModel>();
            SimpleIoc.Default.Register<WizardViewModel>();
            SimpleIoc.Default.Register<DrinkerCycleViewModel>();
        }

        public ProgressViewModel ProgressViewModel => ServiceLocator.Current.GetInstance<ProgressViewModel>();

        public MainPageViewModel MainPageViewModel => ServiceLocator.Current.GetInstance<MainPageViewModel>();
        public WizardViewModel WizardViewModel => ServiceLocator.Current.GetInstance<WizardViewModel>();
        public DrinkerCycleViewModel DrinkerCycleViewModel => ServiceLocator.Current.GetInstance<DrinkerCycleViewModel>();
    }
}
