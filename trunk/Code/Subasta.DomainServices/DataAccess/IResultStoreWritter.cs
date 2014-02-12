using Subasta.Domain.Game;

namespace Subasta.DomainServices.DataAccess
{
	public interface IResultStoreWritter
	{
		void Add(NodeResult result);
	}
}