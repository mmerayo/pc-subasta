using Subasta.Domain.DalModels;

namespace Subasta.DomainServices.Dal
{
	public interface IStoredGameReader
	{
		StoredGameData LoadFromFile(string file);
	}
}