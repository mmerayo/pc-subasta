using Subasta.Domain.Deck;
using Subasta.Domain.Game.Algorithms;
using Subasta.Domain.Game.Analysis;

namespace Subasta.Lib.Infrastructure
{
	class NullExplorationListenerHandler:ISaysExplorationListener
	{
		public void Update(ISuit suit, ITreeNodeInfo treeNodeTeam1, ITreeNodeInfo treeNodeTeam2)
		{
			//DO NOTHING
		}
	}
}