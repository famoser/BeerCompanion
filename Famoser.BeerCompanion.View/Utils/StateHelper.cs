using System.Threading.Tasks;
using Famoser.BeerCompanion.Business.Services;
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
