using System;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Business.Enums;
using Famoser.BeerCompanion.Business.Services;

namespace Famoser.BeerCompanion.Presentation.Droid
{
	public class StorageService : IStorageService
	{
	    public Task<string> GetCachedData()
	    {
	        throw new NotImplementedException();
	    }

	    public Task<string> GetUserInformations()
	    {
	        throw new NotImplementedException();
	    }

	    public Task<string> GetUserBeers()
	    {
	        throw new NotImplementedException();
	    }

	    public Task<string> GetAssetFile(AssetFileKeys fileKey)
	    {
	        throw new NotImplementedException();
	    }

	    public Task<bool> SetCachedData(string data)
	    {
	        throw new NotImplementedException();
	    }

	    public Task<bool> SetUserInformations(string info)
	    {
	        throw new NotImplementedException();
	    }

	    public Task<bool> SetUserBeers(string info)
	    {
	        throw new NotImplementedException();
	    }

	    public Task<bool> ResetApplication()
	    {
	        throw new NotImplementedException();
	    }
	}
}

