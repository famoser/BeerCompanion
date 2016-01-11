using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Business.Models;

namespace Famoser.BeerCompanion.Business.Repository.Interfaces
{
    public interface ISettingsRepository
    {
        Task<UserInformations> GetUserInformations();
        Task<bool> SaveUserInformations(UserInformations uinfo);
        Task<bool> SyncUserInformations(UserInformations uinfo);

        UserInformations GetSampleUserInformations();
    }
}
