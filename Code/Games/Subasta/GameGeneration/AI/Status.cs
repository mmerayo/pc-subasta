using System;
using System.Collections.Generic;
using System.Linq;
using Games.Deck;

namespace Games.Subasta.GameGeneration.AI
{
	class Status
	{
		private readonly ICardComparer _cardsComparer;
		private int _turn=int.MinValue;
		private readonly ICard[][] _playerCards=new ICard[4][];
		private List<IHand> _hands; 

		public Status(ICardComparer cardsComparer,ISuit trump)
		{
			Trump = trump;
			_cardsComparer = cardsComparer;
			PlayerBets = int.MinValue;
		}

		public Status Clone()
		{
			var status = new Status(_cardsComparer, Trump) {_turn = _turn, PlayerBets = PlayerBets};
			Array.Copy(_playerCards, status._playerCards, 4);
			
			status._hands=new List<IHand>();
			_hands.ForEach(x => status._hands.Add(x.Clone()));
			return status;
		}

		public ISuit Trump { get; private set; }
		
		//el jugador que la pone
		public int PlayerBets { get; private set; }

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
			get { return Hands.Last(); }
		}

		public List<IHand> Hands
		{
			get
			{
				if (_hands == null)
				{
					_hands = new List<IHand>(10);
					AddHand();
				}
				return _hands;
			}
		}

		public Declaration[] Declarables
		{
			get
			{
				//TODO: havent been applied yet 
				//and havent been used in the latest completed hand 
				//and in the lates completed hand no se ha cantado 
				//eliminar reyyes etc..
				return new Declaration[0];
			}
		}

		public IHand LastCompletedHand
		{
			get
			{
				if (!Hands[0].IsCompleted)
					return null;
				return Hands.Last(x => x.IsCompleted);

			}
		}


		public void AddHand()
		{
			ThrowIfNotPlayerBetSet();

			if (_hands.Count == 10 || _playerCards.All(x=>x.Length==0))
				return;
			var item = new Hand(_cardsComparer, Trump);
			_hands.Add(item);
		}

		private void ThrowIfNotPlayerBetSet()
		{
			if (PlayerBets == int.MinValue)
				throw new InvalidOperationException("Player bets need to be set");
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

		public void SetPlayerBet(int playerPosition)
		{
			ThrowIfNotValidPlayerPosition(playerPosition);
			PlayerBets = playerPosition;
		}
		
	}
}
