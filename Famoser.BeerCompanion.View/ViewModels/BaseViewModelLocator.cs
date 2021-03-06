﻿using Famoser.BeerCompanion.Business.Repository;
using Famoser.BeerCompanion.Business.Repository.Interfaces;
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

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<WizardViewModel>();
            SimpleIoc.Default.Register<DrinkerCycleViewModel>();

            SimpleIoc.Default.Register<SettingsViewModel>();
        }

        public ProgressViewModel ProgressViewModel => ServiceLocator.Current.GetInstance<ProgressViewModel>();
        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();
        public WizardViewModel WizardViewModel => ServiceLocator.Current.GetInstance<WizardViewModel>();
        public DrinkerCycleViewModel DrinkerCycleViewModel => ServiceLocator.Current.GetInstance<DrinkerCycleViewModel>();
        public SettingsViewModel SettingsViewModel => ServiceLocator.Current.GetInstance<SettingsViewModel>();
    }
}
