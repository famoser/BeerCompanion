using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Famoser.BeerCompanion.Presentation.WindowsUniversal.UserControls.WizardPage
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
