using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Famoser.BeerCompanion.Presentation.Droid.ViewModel;

namespace Famoser.BeerCompanion.Presentation.Droid.Activities
{
    [Activity(Label = "Famoser.BeerCompanion.Presentation.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;
        private static bool _initialized;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Toast.MakeText(this, "Hello World", ToastLength.Short).Show();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };

            if (!_initialized)
            {
                _initialized = true;

                var vml = new ViewModelLocator();
            }
        }
    }
}

