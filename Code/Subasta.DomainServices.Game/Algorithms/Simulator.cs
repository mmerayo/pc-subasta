using System;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.DomainServices.DataAccess;
using Subasta.DomainServices.Game.Models;

namespace Subasta.DomainServices.Game.Algorithms
{
	internal abstract class Simulator
	{
		private readonly ICardComparer _cardComparer;
		private readonly IPlayerDeclarationsChecker _declarationsChecker;
		private readonly IGameSettingsStoreWritter _gameSettingsWritter;

		public Simulator(ICardComparer cardComparer, IPlayerDeclarationsChecker declarationsChecker, IGameSettingsStoreWritter gameSettingsWritter)
		{
			_cardComparer = cardComparer;
			_declarationsChecker = declarationsChecker;
			_gameSettingsWritter = gameSettingsWritter;
		}

		public virtual IExplorationStatus GetInitialStatus(Guid gameId, int firstPlayer, int forPlayerTeamBets, ICard[] cardsP1,
		                                           ICard[] cardsP2, ICard[] cardsP3, ICard[] cardsP4, ISuit trump,
		                                           int pointsBet)
		{
			var status = new Status(gameId, _cardComparer, trump, _declarationsChecker);
			status.SetCards(1, cardsP1);
			status.SetCards(2, cardsP2);
			status.SetCards(3, cardsP3);
			status.SetCards(4, cardsP4);
			status.SetPlayerBet(forPlayerTeamBets, pointsBet);
			status.Turn = firstPlayer;
			_gameSettingsWritter.StoreGameInfo(gameId, firstPlayer, forPlayerTeamBets, trump, cardsP1, cardsP2, cardsP3, cardsP4);

			return status;
		}

		
	}
}