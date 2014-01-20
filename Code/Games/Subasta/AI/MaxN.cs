using System;
using System.Diagnostics;
using System.Linq;
using Games.Deck;

namespace Games.Subasta.AI
{
	class MaxN
	{
		private IValidCardsRule _validMoveRule;

		public NodeResult Execute(Status currentStatus, int playerPosition)
		{
			if (IsTerminalNode(currentStatus, playerPosition))
				return new NodeResult(currentStatus);

			var candidates = GetCandidates(currentStatus, playerPosition);

			Status updatedStatus= PlayCandidate(currentStatus, playerPosition,candidates[0]);
			var best = Execute(updatedStatus, updatedStatus.Turn);
			var length = candidates.Length;
			for (int i = 1; i < length; i++)
			{
				updatedStatus = PlayCandidate(currentStatus, playerPosition, candidates[i]);
				var current = Execute(updatedStatus, updatedStatus.Turn);

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

		private Status PlayCandidate(Status currentStatus, int playerPosition, ICard candidate)
		{
			Status result = currentStatus.Clone();
			var playerCards = result.PlayerCards(playerPosition).ToList();
			int removed = playerCards.RemoveAll(x => x == candidate);
			Debug.Assert(removed==1);

		    result.SetCards(playerPosition,playerCards.ToArray());

			var hand = result.CurrentHand;

			hand.Add(playerPosition,candidate);
			result.Turn = hand.IsCompleted ? hand.PlayerWinner : NextPlayer(result.Turn);
			return result;
		}
		
		private ICard[] GetCandidates(Status currentStatus, int playerPosition)
		{
			return _validMoveRule.GetValidMoves(currentStatus.PlayerCards(playerPosition), currentStatus.CurrentHand);
		}

		private bool IsTerminalNode(Status currentStatus, int playerPosition)
		{
			return currentStatus.PlayerCards(playerPosition).Length == 0;
		}

		internal class NodeResult
		{
			private readonly Status _status;

			public NodeResult(Status status)
			{
				_status = status; //contar y sumar player 1 y 3 y player 2 y 4
				throw new NotImplementedException();
			}

			public int this[int playerPosition]
			{
				get { throw new System.NotImplementedException(); }
			}
		}
	}
}
