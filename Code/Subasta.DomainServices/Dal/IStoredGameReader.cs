using Subasta.Domain.DalModels;

namespace Subasta.DomainServices.Dal
{
	public interface IStoredGameReader
	{
		StoredGameData Load(string file);
	}
}