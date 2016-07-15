using System.Threading.Tasks;
using Famoser.BeerCompanion.Business.Services;

#pragma warning disable 1998
namespace Famoser.BeerCompanion.Presentation.WindowsUniversal.Platform
{
    public class InteractionService : IInteractionService
    {
        public async Task<bool> CanUseInternet()
        {
            return true;
        }

        public async Task<string> GetPersonalName()
        {
            return Windows.Networking.Proximity.PeerFinder.DisplayName;
        }
    }
}
