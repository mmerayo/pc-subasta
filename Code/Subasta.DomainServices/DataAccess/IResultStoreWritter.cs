using System.Collections.Generic;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.DataAccess
{
	public interface IResultStoreWritter
	{
		void Add(NodeResult result);
	    void Add(IEnumerable<NodeResult> result);
	}
}