using Famoser.BeerCompanion.View.Enums;

namespace Famoser.BeerCompanion.View.Services
{
    public interface IProgressService
    {
        void ShowProgress(ProgressKeys key);
        void HideProgress(ProgressKeys key);
    }
}
