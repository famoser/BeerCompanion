using System;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Data.Entities.Communication;
using Famoser.BeerCompanion.Data.Entities.Communication.Generic;

namespace Famoser.BeerCompanion.Data.Services
{
    public interface IDataService
    {
        Task<bool> ApiOnline();

        Task<DrinkerCycleResponse> GetDrinkerCycle(Guid ownId);
        Task<BooleanResponse> PostDrinkerCycle(DrinkerCycleRequest request);

        Task<BeerResponse> GetBeers(Guid ownId);
        Task<BooleanResponse> PostBeer(BeerRequest request);

        Task<DrinkerResponse> GetDrinker(Guid ownId);
        Task<BooleanResponse> PostDrinker(DrinkerRequest request);
    }
}
