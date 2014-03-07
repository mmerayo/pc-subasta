using System;
using System.Collections.Generic;
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
		private int _maxDepth;

		public GameExplorer(IValidCardsRule validMoveRule,
		                    IQueuedResultStoreWritter resultsWritter,
		                    ICardComparer cardComparer,
		                    IPlayerDeclarationsChecker declarationsChecker,
		                    IGameSettingsStoreWritter gameSettingsWritter )
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
		                    ISuit trump, int pointsBet)
		{
			try
			{
				var status = GetInitialStatus(gameId, firstPlayer, forPlayerTeamBets, cardsP1, cardsP2, cardsP3, cardsP4, trump,
				                              pointsBet);
				Execute(status, firstPlayer);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}

		public IExplorationStatus GetInitialStatus(Guid gameId, int firstPlayer, int forPlayerTeamBets, ICard[] cardsP1,
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

		public NodeResult Execute(IExplorationStatus currentStatus)
		{
			return Execute(currentStatus, currentStatus.Turn);
		}

		public int MaxDepth
		{
			get { return _maxDepth; }
			set
			{
				if(value<0) throw new ArgumentOutOfRangeException();
				_maxDepth = value;
			}
		}

		private NodeResult Execute(IExplorationStatus currentStatus, int playerPlays)
		{
			if (IsTerminalNode(currentStatus, playerPlays))
			{
				//Console.WriteLine("***Terminal Node");
				var nodeResult = new NodeResult(currentStatus);
				StoreResult(nodeResult);

				return nodeResult;
			}

			var candidates = GetCandidates(currentStatus, playerPlays);

			IExplorationStatus updatedStatus;
			var best = Explore(currentStatus, playerPlays, candidates[0], out updatedStatus);
			var length = candidates.Length;
			for (int i = 1; i < length; i++)
			{
				var current = Explore(currentStatus, playerPlays, candidates[i], out updatedStatus);

				if (current[playerPlays] > best[playerPlays])
				{
					//Console.WriteLine("---Found better move");
					best = current;
				}
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
			//TODO: TO STRATEGY
			//return _validMoveRule.GetValidMoves(currentStatus.PlayerCards(playerPosition), currentStatus.CurrentHand);

			//TODO: Create filter for non consecutives
			return FilterToMaxMin(_validMoveRule.GetValidMoves(currentStatus.PlayerCards(playerPosition), currentStatus.CurrentHand));

		}

		private static ICard[] FilterToMaxMin(ICard[] source)
		{

			var result = source.ToList();
			foreach (var sameSuit in result.GroupBy(x => x.Suit))
			{
				var ordered = sameSuit.OrderByDescending(x => x.Value).ThenByDescending(x => x.Number).ToList();
				var current = ordered.Skip(1);
				current = current.Take(current.Count() - 1);
				foreach (var card in current)
				{
					result.Remove(card);
					//Console.WriteLine("One Removed");
				}
			}

			return result.ToArray();
		}

		private bool IsTerminalNode(IExplorationStatus currentStatus, int playerPosition)
		{
			//Add the target points
			//playerPosition position is in team bets then the points where reached so the rest is prunned  otherwise ->
			//-> 130 + POTENTIAL DECLARATION POINTS TEAM BETS - BETPOINTS
			bool isInTeamBets = IsInTeamBets(currentStatus.PlayerBets, playerPosition);
			if (isInTeamBets && currentStatus.SumTotalTeam(playerPosition) >= currentStatus.PointsBet) return true;

			//for now never canta
			if (!isInTeamBets && currentStatus.SumTotalTeam(playerPosition) >= 130 - currentStatus.PointsBet) return true;

			return currentStatus.PlayerCards(playerPosition).Length == 0;
		}

		private bool IsInTeamBets(int playerBets, int playerPosition)
		{
			if (playerBets == 1 || playerBets == 3)
				return playerPosition == 1 || playerPosition == 3;
			return playerPosition == 2 || playerPosition == 4;
		}
	}
}