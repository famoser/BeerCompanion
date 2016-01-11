using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Business.Services;
using Famoser.BeerCompanion.Common.Framework.Singleton;
using GalaSoft.MvvmLight.Ioc;

namespace Famoser.BeerCompanion.View.Utils
{
    public class ResetHelper : SingletonBase<ResetHelper>
    {
        public async Task<bool> ResetApplication()
        {
            var ss = await SimpleIoc.Default.GetInstance<IStorageService>().ResetApplication();
            return ss;
        }
    }
}
