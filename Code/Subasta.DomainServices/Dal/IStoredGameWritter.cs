using Subasta.Domain.DalModels;

namespace Subasta.DomainServices.Dal
{
	public interface IStoredGameWritter
	{
		void Write(StoredGameData source);
	}
}