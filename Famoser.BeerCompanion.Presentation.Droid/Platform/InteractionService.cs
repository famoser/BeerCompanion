using System;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Business.Services;

namespace Famoser.BeerCompanion.Presentation.Droid
{
	public class InteractionService : IInteractionService
	{
		public InteractionService ()
		{
		}

        public async Task<bool> CanUseInternet()
        {
            return true;
        }

        public async Task<string> GetPersonalName()
        {
            return "Adroid";
        }
    }
}

