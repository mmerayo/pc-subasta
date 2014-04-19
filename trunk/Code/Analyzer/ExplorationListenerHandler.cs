using System.Collections.Generic;
using StructureMap;
using Subasta.Domain.Deck;
using Subasta.Domain.Game.Algorithms;
using Subasta.Domain.Game.Analysis;

namespace Analyzer
{
	internal class ExplorationListenerHandler : ISaysExplorationListener
	{
		private readonly Dictionary<ISuit,FrmGameExplorationAnalyzer> _targets=new Dictionary<ISuit, FrmGameExplorationAnalyzer>(4);

		public void Update(ISuit suit, ITreeNodeInfo treeNodeTeam1, ITreeNodeInfo treeNodeTeam2)
		{
			FrmGameExplorationAnalyzer target= GetTarget(suit);
			target.Display(treeNodeTeam1,treeNodeTeam2);
		}

		private FrmGameExplorationAnalyzer GetTarget(ISuit suit)
		{
			if(!_targets.ContainsKey(suit))
			{
				var target = ObjectFactory.GetInstance<FrmGameExplorationAnalyzer>();
				target.SetSuit(suit);
				target.Show();

				_targets.Add(suit,target);
			}

			return _targets[suit];
		}
	}
}