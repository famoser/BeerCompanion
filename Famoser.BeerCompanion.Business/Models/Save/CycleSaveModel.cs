using System.Collections.ObjectModel;

namespace Famoser.BeerCompanion.Business.Models.Save
{
    public class CycleSaveModel
    {
        public ObservableCollection<Drinker> Drinkers { get; set; }
        public ObservableCollection<DrinkerCycle> DrinkerCycles { get; set; }
    }
}
