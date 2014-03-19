using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Strategies
{
	internal class CandidatePlayer:ICandidatePlayer
	{
		private int NextPlayer(int playerPosition)
		{
			var result = playerPosition + 1;

			if (result == 5)
				result = 1;

			return result;
		}

		public IExplorationStatus PlayCandidate(IExplorationStatus currentStatus, int playerPosition, ICard candidate)
		{
			IExplorationStatus result = currentStatus.Clone();

			result.RemovePlayerCard(playerPosition, candidate);

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
	}
}