using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Famoser.BeerCompanion.Presentation.WinUniversal.UserControls.MainPage
{
    public sealed partial class UserSettings : UserControl
    {
        public UserSettings()
        {
            this.InitializeComponent();
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            ColorGridView.Visibility = Visibility.Visible;
        }

        private void ColorGridView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            ColorGridView.Visibility = Visibility.Collapsed;
        }
    }
}
