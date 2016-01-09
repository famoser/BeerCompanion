using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
        Task<bool> AddSelf(string name, Guid onwGuid);
        Task<bool> RemoveSelf(string name, Guid onwGuid);

        Task<bool> AuthenticateUser(string name, Guid userGuid);
        Task<bool> DeAuthenticateUser(string name, Guid userGuid);
    }
}
