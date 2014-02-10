using System;
using System.Linq;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.DomainServices.Game;
using Subasta.DomainServices.Store;

namespace Subasta.Infrastructure.DomainServices
{
	internal class GameExplorer : IGameExplorer
	{
		private readonly IValidCardsRule _validMoveRule;
		private readonly IResultStoreWritter _resultsWritter;

		public GameExplorer(IValidCardsRule validMoveRule,IResultStoreWritter resultsWritter)
		{
			if (validMoveRule == null) throw new ArgumentNullException("validMoveRule");
			if (resultsWritter == null) throw new ArgumentNullException("resultsWritter");
			_validMoveRule = validMoveRule;
			_resultsWritter = resultsWritter;
		}

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
        private NodeResult Explore(IExplorationStatus currentStatus, int playerPosition, ICard candidate, out IExplorationStatus updatedStatus)
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
