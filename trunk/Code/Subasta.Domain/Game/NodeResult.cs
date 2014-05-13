using System;
using System.Linq;
using Subasta.Domain.Deck;

namespace Subasta.Domain.Game
{
	public class NodeResult
	{
		private readonly IExplorationStatus _status;

		public int Points1And3 { get; set; }

		public int Points2And4 { get; set; }

		private int GetPoints(byte playerNum1, byte playerNum2)
		{
			int result = Status.SumTotal(playerNum1) + Status.SumTotal(playerNum2);
			if (Status.IsCompleted &&
			    Status.Hands.Count == 10 &&
			    Status.Hands[9].IsCompleted &&
			    (Status.CurrentHand.PlayerWinner == playerNum1 || Status.CurrentHand.PlayerWinner == playerNum2))
				result += 10;
			return result;
		}


		public ICard CardAtMove(byte playerPosition, byte moveNumber)
		{
			return Status.Hands[moveNumber - 1].PlayerCardResolve(playerPosition);
		}

		public NodeResult(IExplorationStatus status)
		{
			_status = status.Clone();
			Points1And3 = GetPoints(1, 3);
			Points2And4 = GetPoints(2, 4);
		}

		public int this[int playerPosition]
		{
			get { return playerPosition == 1 || playerPosition == 3 ? Points1And3 : Points2And4; }
		}

	    public IHand[] Hands
	    {
	        get { return Status.Hands.ToArray(); }
	    }

	    public Guid GameId
	    {
            get { return Status.GameId; }
	    }

	    public ISuit Trump
	    {
	        get { return Status.Trump; }
	    }

		public int TeamBet
		{
			get { return Status.PlayerBets == 1 || Status.PlayerBets == 3 ? 1 : 2; }
		}

		public IExplorationStatus Status
		{
			get { return _status; }
		}

		public Declaration? DeclarationAtMove(int moveNumber)
		{
			return Status.Hands[moveNumber - 1].Declaration;
			
		}

		//this is crap
		public Declaration? FirstDeclarable(int handNum)
		{
			if (Status.Declarables.Length == 0)
			{
				return Status.Hands[handNum - 1].Declaration;
			}
			return Status.Declarables.FirstOrDefault();

		}
	}

}