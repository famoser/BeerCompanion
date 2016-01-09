using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Famoser.BeerCompanion.Common.Services
{
    public interface IDataService
    {
        Task<string> GetDrinkerCycle(Guid ownId);

        Task<bool> PostDrinkerCycle(string json);

        Task<string> GetBeers(Guid ownId);

        Task<bool> PostBeers(string json);

        Task<bool> DeleteBeers(string json);

        Task<bool> UpdateDrinker(string json);
    }
}
