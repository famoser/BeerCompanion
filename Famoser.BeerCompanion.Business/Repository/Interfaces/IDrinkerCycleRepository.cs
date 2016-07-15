using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Business.Models;

namespace Famoser.BeerCompanion.Business.Repository.Interfaces
{
    public interface IDrinkerCycleRepository
    {
        Task<ObservableCollection<DrinkerCycle>> GetSavedCycles();
        Task<bool> SaveCycles(ObservableCollection<DrinkerCycle> drinkerCycles);

        Task<ObservableCollection<DrinkerCycle>> AktualizeCycles();

        ObservableCollection<DrinkerCycle> GetSampleCycles();
        
        //communication
        Task<bool> DoesExists(string name);
        Task<bool> AddSelf(string name);
        Task<bool> RemoveSelf(string name);

        Task<bool> AuthenticateUser(string name, Guid authGuid);
        Task<bool> DeAuthenticateUser(string name, Guid deAuthGuid);
        Task<bool> RemoveUser(string name, Guid deAuthGuid);
    }
}
