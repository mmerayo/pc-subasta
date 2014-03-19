using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.MCTS
{
	class MctsGameExplorer : ISimulator
	{
		public IExplorationStatus GetInitialStatus(Guid gameId, int firstPlayer, int forPlayerTeamBets, ICard[] cardsP1,
		                                           ICard[] cardsP2, ICard[] cardsP3, ICard[] cardsP4, ISuit trump, int pointsBet)
		{
			throw new NotImplementedException();
		}

		public NodeResult Execute(IExplorationStatus currentStatus)
		{
			throw new NotImplementedException();
		}

		public int MaxDepth { get; set; }
	}
}
