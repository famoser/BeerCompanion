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

        Task<bool> PostDrinkeCycle(string json);

        Task<bool> DeleteDrinkeCycle(string json);
        
        Task<bool> PostBeers(string json);

        Task<bool> DeleteBeers(string json);

        Task<bool> GetBeers(Guid ownId);
    }
}
