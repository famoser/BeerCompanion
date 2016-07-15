using Windows.UI.Xaml.Controls;
using Famoser.BeerCompanion.Business.Models;
using Famoser.BeerCompanion.View.ViewModels;
using GalaSoft.MvvmLight.Ioc;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Famoser.BeerCompanion.Presentation.WinUniversal.UserControls.MainPage
{
    public sealed partial class GroupOverView : UserControl
    {
        public GroupOverView()
        {
            this.InitializeComponent();
        }

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var cycle = e.ClickedItem as DrinkerCycle;
            var vm = SimpleIoc.Default.GetInstance<MainViewModel>();
            vm.NavigateToCycle(cycle);
        }
    }
}
