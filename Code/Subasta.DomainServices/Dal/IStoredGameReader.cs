using Subasta.Domain.DalModels;

namespace Subasta.DomainServices.Dal
{
	public interface IStoredGameReader
	{
		StoredGameData LoadFromFile(string file);
	}

	public interface IStoredGameWritter
	{
		void Write(StoredGameData source);
	}

}