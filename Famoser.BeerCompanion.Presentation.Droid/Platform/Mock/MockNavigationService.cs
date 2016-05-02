using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Views;

namespace Famoser.BeerCompanion.Presentation.Droid.Platform.Mock
{
    public class MockNavigationService : INavigationService
    {
        public void GoBack()
        {
            
        }

        public void NavigateTo(string pageKey)
        {

        }

        public void NavigateTo(string pageKey, object parameter)
        {

        }

		public string CurrentPageKey { get; set; }
    }
}