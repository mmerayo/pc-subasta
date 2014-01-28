using System;
using System.Linq;
using Games.Deck;
using Games.Subasta.GameGeneration.AI;

namespace Games.Subasta
{
	internal class Hand : IHand
	{
		private readonly ICard[] _hand = new ICard[4];
		private int _playerWinner = int.MinValue;
		private int _firstPlayer = int.MinValue;
		private readonly ICardComparer _cardsComparer;

		public Hand(ICardComparer cardsComparer, ISuit trump)
		{
			if (cardsComparer == null) throw new ArgumentNullException("cardsComparer");
			_cardsComparer = cardsComparer;
			Trump = trump;
		}

		public int Add(int playerPlays, ICard card)
		{
			ThrowIfNotValidPlayerPosition(playerPlays, "playerPlays");

			if (_firstPlayer == int.MinValue)
				_firstPlayer = playerPlays;

			var index = playerPlays - 1;

			if (_hand[index] != null)
				throw new InvalidOperationException("There is already one card in that position");

			if (_hand.Any(x => x != null && x.Equals(card)))
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
				if (_playerWinner == int.MinValue)
					_playerWinner = GetWinner();

				return _playerWinner;
			}
			private set { _playerWinner = value; }
		}

		public int Points
		{
			get { return _hand.Where(x => x != null).Sum(x => x.Value); }

		}

		public bool IsStartedByTrump
		{
			get
			{
				ThrowIfEmpty();
				return _hand[_firstPlayer - 1].Suit==Trump;
			}
		}

		private void ThrowIfEmpty()
		{
			if (IsEmpty)
				throw new InvalidOperationException("Start before this call");

		}

		public ISuit StartedBySuit
		{
			get
			{
				ThrowIfEmpty();
				return _hand[_firstPlayer - 1].Suit;
			}
		}

		public ICard CardWinner
		{
			get { return GetCurrentCardWinner(); }
		}

		public bool IsEmpty
		{
			get { return _firstPlayer == int.MinValue; }
		}

		public bool BrokeToTrump
		{
			get
			{
				ThrowIfEmpty();
				if (_hand[_firstPlayer - 1].Suit == Trump)
					return false;
				return _hand.Any(x => x!=null &&  x.Suit == Trump);
			}
		}

		public ISuit Trump { get; private set; }
		public ICard PlayerCard(int playerPosition)
		{
			return _hand[playerPosition - 1];
		}

		public IHand Clone()
		{
			var result=new Hand(_cardsComparer,Trump)
				{
					_playerWinner=this._playerWinner,
					_firstPlayer = this._firstPlayer,
				};
			Array.Copy(_hand,result._hand,4);
			return result;
		}

		public void Add(Declaration declaration)
		{
			throw new NotImplementedException();
		}


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


		private int GetWinner()
		{
			var currentWin = GetCurrentCardWinner();

			return Array.IndexOf(_hand, currentWin) + 1;
		}

		private ICard GetCurrentCardWinner()
		{
			ThrowIfEmpty();

			var currentPlayer = _firstPlayer;

			var currentWin = _hand[currentPlayer - 1];

			do
			{
				currentPlayer = NextPlayer(currentPlayer);
				var card = _hand[currentPlayer - 1];

				if (card == null) break;

				currentWin = _cardsComparer.Best(currentWin, card);
			} while (currentPlayer != _firstPlayer);
			return currentWin;
		}


		private static int NextPlayer(int currentPlayer)
		{
			if (++currentPlayer > 4)
				currentPlayer = 1;
			return currentPlayer;
		}

		public override string ToString()
		{
			return string.Format("a-{0} - b-{1} - c-{2} - d-{3}", GetCardString(0), GetCardString(1), GetCardString(2),
			                     GetCardString(3));
		}

		private string GetCardString(int index)
		{
			if (_hand[index] == null) return "null";
			return _hand[index].ToString();
		}

		
	}
}