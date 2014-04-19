using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Subasta.Domain.Deck;
using Subasta.Domain.Game.Algorithms;

namespace Subasta.Domain.Game.Analysis
{
	public interface ISaysExplorationListener
	{
		void Update(ISuit suit, ITreeNodeInfo treeNodeTeam1, ITreeNodeInfo treeNodeTeam2);
	}
}
