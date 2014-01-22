using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Games.Deck;

namespace Games.Subasta.AI
{
	class Status
	{
		private readonly ICardComparer _cardsComparer;
		private int _turn=int.MinValue;
		private readonly List<ICard[]> _playerCards=new List<ICard[]>(4);
		private readonly List<IHand> _hands=new List<IHand>(10); 

		public Status(ICardComparer cardsComparer)
		{
			_cardsComparer = cardsComparer;
			for(int i=0;i<4;i++)
				_playerCards.Add(null);

			_hands.Add(new Hand(cardsComparer));
		}

		public Status Clone()
		{
			var status = new Status(_cardsComparer) {_turn = _turn};
			status._playerCards.AddRange(_playerCards);
			status._hands.AddRange(_hands);
			return status;
		}

		public int Turn
		{
			set
			{
				ThrowIfNotValidPlayerPosition(value, "value");
				_turn = value;
			}
			get
			{
				if (_turn == int.MinValue)
					throw new InvalidOperationException("Turn has not been set");
				return _turn;
			}
		}

		public IHand CurrentHand
		{
			get { return _hands.Last(); }
		}

		public List<IHand> Hands
		{
			get { return _hands; }
		}


		public ICard[] PlayerCards(int playerPosition)
		{
			ThrowIfNotValidPlayerPosition(playerPosition);
			return _playerCards[playerPosition-1];
		}

		public void SetCards(int playerPosition, ICard[] cards)
		{
			ThrowIfNotValidPlayerPosition(playerPosition);

			_playerCards[playerPosition - 1] = cards;
		}


		/// <summary>
		/// Sum by player
		/// </summary>
		/// <param name="playerPosition"></param>
		/// <returns></returns>
		public int SumTotal(int playerPosition)
		{
			ThrowIfNotValidPlayerPosition(playerPosition);

			return Hands.Where(x => x.IsCompleted && x.PlayerWinner == playerPosition).Sum(x => x.Points);
		}

		private static void ThrowIfNotValidPlayerPosition(int playerPosition,string argName="playerPosition")
		{
			if (playerPosition < 1 || playerPosition > 4)
				throw new ArgumentOutOfRangeException(argName);
		}
	}
}
