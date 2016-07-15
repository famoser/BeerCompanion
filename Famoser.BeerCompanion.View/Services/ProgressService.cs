using Famoser.BeerCompanion.View.Enums;
using Famoser.BeerCompanion.View.ViewModels;
using GalaSoft.MvvmLight.Ioc;

namespace Famoser.BeerCompanion.View.Services
{
    public class ProgressService : IProgressService
    {
        private readonly ProgressViewModel _viewModel;
        public ProgressService()
        {
            _viewModel = SimpleIoc.Default.GetInstance<ProgressViewModel>();
        }

        public void ShowProgress(ProgressKeys key)
        {
            _viewModel.SetProgressState(key, true);
        }

        public void HideProgress(ProgressKeys key)
        {
            _viewModel.SetProgressState(key, false);
        }
    }
}
