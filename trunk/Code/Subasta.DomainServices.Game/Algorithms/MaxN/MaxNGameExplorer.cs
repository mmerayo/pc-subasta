using System;
using System.Linq;
using Subasta.Domain;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.DomainServices.DataAccess;
using Subasta.DomainServices.Game.Models;

namespace Subasta.DomainServices.Game.Algorithms
{
	internal class MaxNSimulator : ISimulator
	{
		private readonly ICandidatesSelector _candidatesSelector;
		private readonly IResultStoreWritter _resultsWritter;
		private readonly ICardComparer _cardComparer;
		private readonly IPlayerDeclarationsChecker _declarationsChecker;
		private readonly IGameSettingsStoreWritter _gameSettingsWritter;
		private int _maxDepth;

		public MaxNSimulator(ICandidatesSelector candidatesSelector,
		                    IQueuedResultStoreWritter resultsWritter,
		                    ICardComparer cardComparer,
		                    IPlayerDeclarationsChecker declarationsChecker,
		                    IGameSettingsStoreWritter gameSettingsWritter )
		{
			
			if (resultsWritter == null) throw new ArgumentNullException("resultsWritter");
			_candidatesSelector = candidatesSelector;
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
			NodeResult nodeResult = Execute(currentStatus, currentStatus.Turn);
			return nodeResult;
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

			var candidates =_candidatesSelector. GetCandidates(currentStatus, playerPlays);

			IExplorationStatus updatedStatus;
			var best = Explore(currentStatus, playerPlays, candidates[0], out updatedStatus);
			var length = candidates.Length;
			for (int i = 1; i < length; i++)
			{
				ICard candidate = candidates[i];
				var current = Explore(currentStatus, playerPlays, candidate, out updatedStatus);

				int currentPoints = current[playerPlays];
				int currentBestPoints = best[playerPlays];
				if (currentPoints > currentBestPoints || (currentPoints == currentBestPoints && candidate.IsAbsSmallerThan(best.CardAtMove(playerPlays,currentStatus.CurrentHand.Sequence) )))
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
				AddDeclaration(declarables[0], newStatus);

			var best = Execute(newStatus, newStatus.Turn);

			int length = declarables.Length;
			for (int index = 1; index < length; index++)
			{
				var declaration = declarables[index];
				newStatus = updatedStatus.Clone();
				AddDeclaration(declaration, newStatus);

				var current = Execute(newStatus, newStatus.Turn);

				if (current[playerPosition] > best[playerPosition])
					best = current;

			}

			return best;
		}

		private static void AddDeclaration(Declaration declaration, IExplorationStatus newStatus)
		{
			IHand firstDeclarableHand = newStatus.FirstDeclarableHand;
			if (firstDeclarableHand != null && newStatus.IsInTeamBets(firstDeclarableHand.PlayerWinner.Value))
				firstDeclarableHand.Add(declaration);
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

			result.RemovePlayerCard(playerPosition,candidate);

			var hand = result.CurrentHand;

			hand.Add(playerPosition, candidate);

			if (hand.IsCompleted)
			{
				result.Turn = hand.PlayerWinner.Value;
				result.AddNewHand();
			}
			else
				result.Turn = NextPlayer(result.Turn);
			return result;
		}

		private bool IsTerminalNode(IExplorationStatus currentStatus, int playerPosition)
		{
		    if (currentStatus.Hands.Count(x => x.IsCompleted) == MaxDepth) return true;

			return currentStatus.IsCompleted;

		    //return currentStatus.PlayerCards(playerPosition).Length == 0;
		}
	    
	}
}