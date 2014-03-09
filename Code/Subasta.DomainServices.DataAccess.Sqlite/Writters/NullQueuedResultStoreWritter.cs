using System.Collections.Generic;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.DataAccess.Sqlite.Writters
{
    class NullQueuedResultStoreWritter:IQueuedResultStoreWritter
    {
        public void Add(NodeResult result)
        {
        }

        public void Add(IEnumerable<NodeResult> result)
        {
        }
    }
}