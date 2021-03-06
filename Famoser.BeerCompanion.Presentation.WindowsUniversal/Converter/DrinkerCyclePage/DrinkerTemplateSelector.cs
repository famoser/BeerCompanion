﻿using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Famoser.BeerCompanion.Business.Models;
using Famoser.BeerCompanion.View.ViewModels;
using GalaSoft.MvvmLight.Ioc;

namespace Famoser.BeerCompanion.Presentation.WindowsUniversal.Converter.DrinkerCyclePage
{
    class DrinkerTemplateSelector : DataTemplateSelector
    {
        public DataTemplate AuthTemplate { get; set; }
        public DataTemplate NonAuthTemplate { get; set; }
        public DataTemplate OwnMitgliedTemplate { get; set; }

        public DrinkerCycle DrinkerCycle => SimpleIoc.Default.GetInstance<DrinkerCycleViewModel>().DrinkerCycle;

        protected override DataTemplate SelectTemplateCore(object item)
        {
            var type = item as Drinker;
            if (type == null)
                return OwnMitgliedTemplate;
            if (DrinkerCycle?.AuthBeerDrinkers != null && DrinkerCycle.AuthBeerDrinkers.Any(a => a == type))
                return AuthTemplate;
            return NonAuthTemplate;
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return SelectTemplateCore(item);
        }
    }
}
