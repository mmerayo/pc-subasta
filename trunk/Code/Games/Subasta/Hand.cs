using System;
using System.Linq;
using Games.Deck;

namespace Games.Subasta
{
	class Hand : IHand
	{
		private readonly ICard[] _hand=new ICard[4];
		private int _playerWinner;

		public int Add(int playerPlays, ICard card)
		{
			ThrowIfNotValidPlayerPosition(playerPlays,"playerPlays");
			
			var index = playerPlays - 1;
			
			if(_hand[index]!=null)
				throw new InvalidOperationException("There is already one card in that position");

			if(_hand.Any(x=>x!=null && x.Equals(card)))
				throw new InvalidOperationException("Cannot add the same card twice");
			_hand[index] = card;
			return playerPlays;
		}

		public bool IsCompleted
		{
			get { return _hand.All(x => x != null); }
		}

		public int PlayerWinner
		{
			get
			{
				ThrowIfNotcompleted();
				return _playerWinner;
			}
			private set { _playerWinner = value; }
		}

		public int Points { get; private set; }

		private void ThrowIfNotcompleted()
		{
			if (!IsCompleted)
				throw new InvalidOperationException("Use IsCompleted first to ensure that this method is accessible");
		}

		private static void ThrowIfNotValidPlayerPosition(int playerPosition, string argName = "playerPosition")
		{
			if (playerPosition < 1 || playerPosition > 4)
				throw new ArgumentOutOfRangeException(argName);
		}

	}
}