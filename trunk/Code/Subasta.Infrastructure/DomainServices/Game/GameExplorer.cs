using System;
using System.Linq;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.DomainServices.DataAccess;
using Subasta.DomainServices.Game;
using Subasta.Infrastructure.Domain;

namespace Subasta.Infrastructure.DomainServices.Game
{
	internal class GameExplorer : IGameExplorer
	{
		private readonly IValidCardsRule _validMoveRule;
		private readonly IResultStoreWritter _resultsWritter;
		private readonly ICardComparer _cardComparer;
		private readonly IPlayerDeclarationsChecker _declarationsChecker;
		private readonly IGameSettingsStoreWritter _gameSettingsWritter;

		public GameExplorer(IValidCardsRule validMoveRule, IQueuedResultStoreWritter resultsWritter,
		                    ICardComparer cardComparer, IPlayerDeclarationsChecker declarationsChecker,
		                    IGameSettingsStoreWritter gameSettingsWritter)
		{
			if (validMoveRule == null) throw new ArgumentNullException("validMoveRule");
			if (resultsWritter == null) throw new ArgumentNullException("resultsWritter");
			_validMoveRule = validMoveRule;
			_resultsWritter = resultsWritter;
			_cardComparer = cardComparer;
			_declarationsChecker = declarationsChecker;
			_gameSettingsWritter = gameSettingsWritter;
		}

		public void Execute(Guid gameId, int firstPlayer, int forPlayerTeamBets, ICard[] cardsP1, ICard[] cardsP2,
		                    ICard[] cardsP3,
		                    ICard[] cardsP4,
		                    ISuit trump)
		{
			try
			{
				var status = new Status(gameId, _cardComparer, trump, _declarationsChecker);
				status.SetCards(1, cardsP1);
				status.SetCards(2, cardsP2);
				status.SetCards(3, cardsP3);
				status.SetCards(4, cardsP4);
				status.SetPlayerBet(forPlayerTeamBets);
				status.Turn = firstPlayer;
				_gameSettingsWritter.StoreGameInfo(gameId, firstPlayer, forPlayerTeamBets, trump, cardsP1, cardsP2, cardsP3, cardsP4);
				Execute(status, firstPlayer);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}

		//TODO: REMOVE COMPARISONS AS EVERYTHING IS STORED
		public NodeResult Execute(IExplorationStatus currentStatus, int playerPosition)
		{
			if (IsTerminalNode(currentStatus, playerPosition))
			{
				var nodeResult = new NodeResult(currentStatus);
				StoreResult(nodeResult);

				return nodeResult;
			}

			var candidates = GetCandidates(currentStatus, playerPosition);

			IExplorationStatus updatedStatus;
			var best = Explore(currentStatus, playerPosition, candidates[0], out updatedStatus);
			var length = candidates.Length;
			for (int i = 1; i < length; i++)
			{
				var current = Explore(currentStatus, playerPosition, candidates[i], out updatedStatus);

				if (current[playerPosition] > best[playerPosition])
					best = current;
			}

			return best;
		}

		private void StoreResult(NodeResult result)
		{
			_resultsWritter.Add(result);
		}

		//explora los cantes tambien
		private NodeResult Explore(IExplorationStatus currentStatus, int playerPosition, ICard candidate,
		                           out IExplorationStatus updatedStatus)
		{
			updatedStatus = PlayCandidate(currentStatus, playerPosition, candidate);

			var declarables = updatedStatus.Declarables;
			var newStatus = updatedStatus.Clone();
			if (declarables.Length > 0)
				newStatus.LastCompletedHand.Add(declarables[0]);

			var best = Execute(newStatus, newStatus.Turn);

			int length = declarables.Length;
			for (int index = 1; index < length; index++)
			{
				var declaration = declarables[index];
				newStatus = updatedStatus.Clone();
				newStatus.LastCompletedHand.Add(declaration);

				var current = Execute(newStatus, newStatus.Turn);

				if (current[playerPosition] > best[playerPosition])
					best = current;

			}

			return best;
		}


		private int NextPlayer(int playerPosition)
		{
			var result = playerPosition + 1;

			if (result == 5)
				result = 1;

			return result;
		}

		private IExplorationStatus PlayCandidate(IExplorationStatus currentStatus, int playerPosition, ICard candidate)
		{
			IExplorationStatus result = currentStatus.Clone();
			var playerCards = result.PlayerCards(playerPosition).ToList();
			playerCards.RemoveAt(playerCards.IndexOf(candidate));

			result.SetCards(playerPosition, playerCards.ToArray());

			var hand = result.CurrentHand;

			hand.Add(playerPosition, candidate);

			if (hand.IsCompleted)
			{
				result.Turn = hand.PlayerWinner;
				result.AddNewHand();
			}
			else
				result.Turn = NextPlayer(result.Turn);
			return result;
		}

		private ICard[] GetCandidates(IExplorationStatus currentStatus, int playerPosition)
		{
			return _validMoveRule.GetValidMoves(currentStatus.PlayerCards(playerPosition), currentStatus.CurrentHand);
		}

		private bool IsTerminalNode(IExplorationStatus currentStatus, int playerPosition)
		{
			return currentStatus.PlayerCards(playerPosition).Length == 0;
		}


	}
}