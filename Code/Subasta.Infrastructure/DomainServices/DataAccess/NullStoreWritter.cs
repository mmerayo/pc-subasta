using System.Collections.Generic;
using Subasta.Domain.Game;
using Subasta.DomainServices.DataAccess;

namespace Subasta.Infrastructure.DomainServices.DataAccess
{
	class NullStoreWritter:IQueuedResultStoreWritter
	{
		public void Add(NodeResult result)
		{
		}

		public void Add(IEnumerable<NodeResult> result)
		{
		}
	}
}