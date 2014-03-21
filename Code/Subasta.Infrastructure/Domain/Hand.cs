using System;
using System.Collections.Generic;
using System.Linq;
using Subasta.Domain;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.DomainServices;
using Subasta.DomainServices.Game;

namespace Subasta.Infrastructure.Domain
{
	internal class Hand : IHand
	{
		private readonly ICard[] _hand = new ICard[4];
		private int? _playerWinner = null;
	    private readonly ICardComparer _cardsComparer;

		public Hand(ICardComparer cardsComparer, ISuit trump,int sequence)
		{
		    FirstPlayer = int.MinValue;
		    if (cardsComparer == null) throw new ArgumentNullException("cardsComparer");
			_cardsComparer = cardsComparer;
			Trump = trump;
		    Sequence = sequence;
		}

		public int Add(int playerPlays, ICard card)
		{
			ThrowIfNotValidPlayerPosition(playerPlays, "playerPlays");

			if (FirstPlayer == int.MinValue)
				FirstPlayer = playerPlays;

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

		public int? PlayerWinner
		{
			get
			{
				
				if (IsCompleted && !_playerWinner.HasValue)
					_playerWinner = GetWinner();

				return _playerWinner;
			}
			private set
			{
				_playerWinner = value;
			}
		}

		public int Points
		{
			get
			{
				return _hand.Where(x => x != null).Sum(x => x.Value)+DeclarationValue;
			}
		}

		public int DeclarationValue
		{
			get
			{
				int result = 0;
				if (Declaration.HasValue)
					return DeclarationValues.ValueOf(Declaration.Value);

				return result;
			}
		}

		public bool IsStartedByTrump
		{
			get
			{
				ThrowIfEmpty();
				return _hand[FirstPlayer - 1].Suit==Trump;
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
				return _hand[FirstPlayer - 1].Suit;
			}
		}

		public ICard CardWinner
		{
			get { return GetCurrentCardWinner(); }
		}

		public bool IsEmpty
		{
			get { return FirstPlayer == int.MinValue; }
		}

		public bool BrokeToTrump
		{
			get
			{
				if (IsEmpty) return false;
				if (_hand[FirstPlayer - 1].Suit == Trump)
					return false;
				return _hand.Any(x => x!=null &&  x.Suit == Trump);
			}
		}

		public ISuit Trump { get; private set; }
	    public int Sequence { get; private set; }

	    public Declaration? Declaration { get; private set; }

	    public int FirstPlayer { get; private set; }

	    public ICard PlayerCard(int playerPosition)
		{
			return _hand[playerPosition - 1];
		}

		public IHand Clone()
		{
			var result=new Hand(_cardsComparer,Trump,Sequence)
				{
					_playerWinner=this._playerWinner,
					FirstPlayer = this.FirstPlayer,
					Declaration = this.Declaration
				};
			Array.Copy(_hand,result._hand,4);
			return result;
		}

		public void SetDeclaration(Declaration declaration)
		{
			ThrowIfNotcompleted();
			Declaration = declaration;
		}

		public IEnumerable<ICard> CardsByPlaySequence()
		{
			var result = new ICard[4];
			if(FirstPlayer==int.MinValue)
				return result;
			int currentIndex = FirstPlayer - 1;
			for (int resultIndex = 0; resultIndex < 4; resultIndex++)
			{
				result[resultIndex] = _hand[currentIndex];
				if (++currentIndex == 4) currentIndex = 0;
			}

			return result;
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

			var currentPlayer = FirstPlayer;

			var currentWin = _hand[currentPlayer - 1];
			currentPlayer = NextPlayer(currentPlayer);

			do
			{
				var card = _hand[currentPlayer - 1];

				if (card == null) break;
				
				currentWin = _cardsComparer.Best(Trump,currentWin, card);
			} while ((currentPlayer = NextPlayer(currentPlayer)) != FirstPlayer);
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