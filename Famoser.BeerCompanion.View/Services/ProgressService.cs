using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.BeerCompanion.View.Enums;
using Famoser.BeerCompanion.View.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;

namespace Famoser.BeerCompanion.View.Services
{
    public class ProgressService : IProgressService
    {
        private ProgressViewModel _viewModel;
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
