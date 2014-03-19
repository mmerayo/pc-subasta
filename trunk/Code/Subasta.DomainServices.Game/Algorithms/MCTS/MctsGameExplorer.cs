using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.DomainServices.DataAccess;

namespace Subasta.DomainServices.Game.Algorithms.MCTS
{
	class MctsGameExplorer :Simulator, ISimulator
	{
		private readonly IMctsRunner _mctsRunner;
		private readonly IMctsTreeCommands _treeQuery;

		public MctsGameExplorer(ICardComparer cardComparer, 
			IPlayerDeclarationsChecker declarationsChecker, 
			IGameSettingsStoreWritter gameSettingsWritter,
			IMctsRunner mctsRunner,IMctsTreeCommands treeQuery)
			: base(cardComparer, declarationsChecker, gameSettingsWritter)
		{
			_mctsRunner = mctsRunner;
			_treeQuery = treeQuery;
		}

		public override IExplorationStatus GetInitialStatus(Guid gameId, int firstPlayer, int forPlayerTeamBets, ICard[] cardsP1, ICard[] cardsP2, ICard[] cardsP3, ICard[] cardsP4, ISuit trump, int pointsBet)
		{
			var result= base.GetInitialStatus(gameId, firstPlayer, forPlayerTeamBets, cardsP1, cardsP2, cardsP3, cardsP4, trump, pointsBet);

			_mctsRunner.Start(result);

			return result;
		}

		public NodeResult GetBest(IExplorationStatus currentStatus)
		{
			return _treeQuery.GetBestFoundAndShallow();
		}

		public int MaxDepth { get; set; }
	}
}
