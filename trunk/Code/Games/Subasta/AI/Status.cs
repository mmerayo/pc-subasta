﻿using System;
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
		private readonly ICard[][] _playerCards=new ICard[4][];
		private List<IHand> _hands; 

		public Status(ICardComparer cardsComparer,ISuit trump)
		{
			Trump = trump;
			_cardsComparer = cardsComparer;
		}

		public Status Clone()
		{
			var status = new Status(_cardsComparer,Trump) {_turn = _turn};
			Array.Copy(_playerCards, status._playerCards, 4);
			
			status._hands=new List<IHand>();
			_hands.ForEach(x => status._hands.Add(x.Clone()));
			return status;
		}

		public ISuit Trump { get; private set; }


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

		public void AddHand()
		{
			if (_hands.Count == 10 || _playerCards.All(x=>x.Length==0))
				return;
			var item = new Hand(_cardsComparer, Trump);
			_hands.Add(item);
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
