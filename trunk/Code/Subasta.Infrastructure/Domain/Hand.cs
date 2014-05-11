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
		private byte? _playerWinner = null;
	    private readonly ICardComparer _cardsComparer;
		private byte? _firstPlayer=null;

		public Hand(ICardComparer cardsComparer, ISuit trump,byte sequence)
		{
		    if (cardsComparer == null) throw new ArgumentNullException("cardsComparer");
			_cardsComparer = cardsComparer;
			Trump = trump;
		    Sequence = sequence;
		}

		public byte Add(byte playerPlays, ICard card)
		{
			ThrowIfNotValidPlayerPosition(playerPlays, "playerPlays");

			if (!_firstPlayer.HasValue)
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

		public byte? PlayerWinner
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

		public byte? TeamWinner
		{
			get
			{
				if (!PlayerWinner.HasValue) return null;
				return (byte) (PlayerWinner.Value == 1 || PlayerWinner.Value == 3 ? 1 : 2);
			}
		}

		public byte Points
		{
			get
			{
				return (byte)(_hand.Where(x => x != null).Sum(x => x.Value)+DeclarationValue);
			}
		}

		public byte DeclarationValue
		{
			get
			{
				byte result = 0;
				if (Declaration.HasValue)
					return DeclarationValues.ValueOf(Declaration.Value);

				return result;
			}
		}

		public byte LastPlayerPlayed
		{
			get
			{
				ThrowIfEmpty();
				byte currPlayer = FirstPlayer;

				do
				{

					currPlayer = NextPlayer(currPlayer);
				} while (PlayerCard(currPlayer) != null && currPlayer != FirstPlayer);
				return PreviousPlayer(currPlayer);
			}
		}

		public byte NumberCardsPlayed
		{
			get { return (byte)CardsByPlaySequence().Count(x => x != null); }
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
			get { return !_firstPlayer.HasValue; }
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

		public byte LastPlayer
		{
			get
			{
				if (FirstPlayer == int.MinValue) throw new InvalidOperationException();
				var firstPlayer = FirstPlayer;

				var current = firstPlayer;
				for (int i = 0; i < 3; i++)
					current = NextPlayer(current);

				return current;

			}
		}

		public byte Sequence { get; private set; }

	    public Declaration? Declaration { get; private set; }

		public byte FirstPlayer
		{
			get { return _firstPlayer.Value; }
		}

		public ICard PlayerCard(int playerPosition)
		{
			return _hand[playerPosition - 1];
		}

		public IHand Clone()
		{
			var result=new Hand(_cardsComparer,Trump,Sequence)
				{
					_playerWinner=this._playerWinner,
					_firstPlayer = this._firstPlayer,
					Declaration = this.Declaration
				};
			Array.Copy(_hand,result._hand,4);
			return result;
		}

		public void SetDeclaration(Declaration? declaration)
		{
			ThrowIfNotcompleted();
			Declaration = declaration;
		}

		public IEnumerable<ICard> CardsByPlaySequence()
		{
			var result = new ICard[4];
			if(!_firstPlayer.HasValue)
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


		private byte GetWinner()
		{
			var currentWin = GetCurrentCardWinner();

			return (byte)(Array.IndexOf(_hand, currentWin) + 1);
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


		private static byte NextPlayer(byte currentPlayer)
		{
			if (++currentPlayer > 4)
				currentPlayer = 1;
			return currentPlayer;
		}

		private byte PreviousPlayer(byte currentPlayer)
		{
			if (--currentPlayer < 1)
				currentPlayer = 4;
			return currentPlayer;
		}

		public override string ToString()
		{
		    string result = string.Format("a-{0} - b-{1} - c-{2} - d-{3}", GetCardString(0), GetCardString(1), GetCardString(2),
		        GetCardString(3));
		    if (Declaration.HasValue)
		        result += string.Format(" - Declared: {0}", Declaration.Value.ToString());
		    return result;
		}

	    private string GetCardString(int index)
		{
			if (_hand[index] == null) return "null";
			return _hand[index].ToString();
		}


		public override bool Equals(Object obj)
			{
			if (obj == null)
				{
				return false;
				}

			var other = obj as Hand;
			if (other == null)
				{
				return false;
				}
			return Equals(other);

			}

		public bool Equals(Hand other)
		{
			if ((object) other == null)
			{
				return false;
			}

			var cards = CardsByPlaySequence().ToArray();
			ICard[] otherCards = other.CardsByPlaySequence().ToArray();

			for (int index = 0; index < cards.Length; index++)
			{
				var card = cards[index];
				var otherCard = otherCards[index];
				if(card!=otherCard)
					return false;
			}
			return (Declaration == other.Declaration);

		}



		public static bool operator ==(Hand a, Hand b)
			{
			if (ReferenceEquals(a, b))
				return true;

			if (((object)a == null) || ((object)b == null))
				return false;

			return a.Equals(b);
			}

		public static bool operator !=(Hand a, Hand b)
			{
			return !(a == b);
			}

	}
}