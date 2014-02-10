using Subasta.Domain.Game;

namespace Subasta.DomainServices.Store
{
	public interface IResultStoreWritter
	{
		void Add(NodeResult result);
	}
}