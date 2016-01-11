using System;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Data.Entities.Communication;

namespace Famoser.BeerCompanion.Data.Services
{
    public interface IDataService
    {
        Task<DrinkerCycleResponse> GetDrinkerCycle(Guid ownId);

        Task<bool> PostDrinkerCycle(DrinkerCycleRequest request);

        Task<BeerResponse> GetBeers(Guid ownId);

        Task<bool> PostBeers(BeerRequest request);

        Task<bool> UpdateDrinker(DrinkerRequest request);
    }
}
