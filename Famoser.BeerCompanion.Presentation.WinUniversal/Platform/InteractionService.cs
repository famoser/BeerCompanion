using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Famoser.BeerCompanion.Business.Services;

namespace Famoser.BeerCompanion.Presentation.WinUniversal.Platform
{
    public class InteractionService : IInteractionService
    {
#pragma warning disable 1998
        public async Task<bool> CanUseInternet()
        {
            return true;
        }

        public async Task<string> GetPersonalName()
        {
            return Windows.Networking.Proximity.PeerFinder.DisplayName;
        }
#pragma warning restore 1998
    }
}
