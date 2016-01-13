using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.BeerCompanion.View.Enums;
using GalaSoft.MvvmLight;

namespace Famoser.BeerCompanion.View.ViewModels
{
    public class ProgressViewModel : ViewModelBase
    {
        private readonly Dictionary<ProgressKeys, bool> _progress = new Dictionary<ProgressKeys, bool>();
        public ProgressViewModel()
        {
            if (IsInDesignMode)
                SetProgressState(ProgressKeys.InDebugMode, true);
        }

        public void SetProgressState(ProgressKeys key, bool state)
        {
            if (_progress.ContainsKey(key))
                _progress[key] = state;
            else
                _progress.Add(key, state);
            RaisePropertyChanged(() => ActiveProgress);
            RaisePropertyChanged(() => IsProgressActive);
        }

        public bool IsProgressActive
        {
            get { return _progress.Any(p => p.Value); }
        }

        public ProgressKeys? ActiveProgress
        {
            get
            {
                return _progress.Where(e => e.Value)
                .Select(e => (KeyValuePair<ProgressKeys, bool>?)e)
                .FirstOrDefault()?.Key;
            }
        }
    }
}
