using System;
using System.Diagnostics;
using System.Linq;
using Games.Deck;

namespace Games.Subasta.AI
{
	internal class MaxN
	{
		private readonly IValidCardsRule _validMoveRule;

		public MaxN(IValidCardsRule validMoveRule)
		{
			if (validMoveRule == null) throw new ArgumentNullException("validMoveRule");
			_validMoveRule = validMoveRule;
		}

		public NodeResult Execute(Status currentStatus, int playerPosition)
		{
			if (IsTerminalNode(currentStatus, playerPosition))
				return new NodeResult(currentStatus);

			var candidates = GetCandidates(currentStatus, playerPosition);

			Status updatedStatus = PlayCandidate(currentStatus, playerPosition, candidates[0]);
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
			playerCards.RemoveAt(playerCards.IndexOf(candidate));

			result.SetCards(playerPosition, playerCards.ToArray());

			var hand = result.CurrentHand;

			hand.Add(playerPosition, candidate);


			if (hand.IsCompleted)
			{
				result.Turn = hand.PlayerWinner;
				result.AddHand();
			}
			else 
				result.Turn = NextPlayer(result.Turn);
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

			public int Points1And3 { get; private set; }
			public int Points2And4 { get; private set; }

			public ICard CardAtMove(int playerPosition,int moveNumber)
			{
				return _status.Hands[moveNumber-1].PlayerCard(playerPosition);
			}

			public NodeResult(Status status)
			{
				_status = status;

				Points1And3 = _status.SumTotal(1) + _status.SumTotal(3);
				Points2And4 = _status.SumTotal(2) + _status.SumTotal(4);
			}

			public int this[int playerPosition]
			{
				get { return playerPosition == 1 || playerPosition == 3 ? Points1And3 : Points2And4; }
			}
		}
	}
}
