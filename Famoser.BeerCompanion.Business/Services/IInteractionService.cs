using System.Threading.Tasks;

namespace Famoser.BeerCompanion.Business.Services
{
    public interface IInteractionService
    {
        Task<bool> CanUseInternet();
        Task<string> GetPersonalName();
    }
}
