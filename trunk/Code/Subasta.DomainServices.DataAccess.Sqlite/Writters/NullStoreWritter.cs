using System.Collections.Generic;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.DataAccess.Sqlite.Writters
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