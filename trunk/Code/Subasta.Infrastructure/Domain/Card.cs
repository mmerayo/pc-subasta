using System;
using Subasta.Domain.Deck;

namespace Subasta.Infrastructure.Domain
{
	public sealed class Card : ICard
	{
		internal Card(ISuit suit, int number)
		{
			Suit = suit;
			Number = number;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shortId">O1,E10</param>
        internal Card(string shortId)
        {
            Suit=Domain.Suit.FromId(shortId[0]);
            Number = int.Parse(shortId.Substring(1));
        }

	    public Card(string suitName, int number)
            : this(Domain.Suit.FromName(suitName), number)
		{
			if (number == 8 || number == 9)
				throw new ArgumentOutOfRangeException("number");
		}


		private int GetValue(int number)
		{
			int result;
			switch (number)
			{
				case 1:
					result = 11;
					break;
				case 3:
					result = 10;
					break;
				case 10:
					result = 2;
					break;
				case 11:
					result = 3;
					break;
				case 12:
					result = 4;
					break;

				case 2:
				case 4:
				case 5:
				case 6:
				case 7:
					result = 0;
					break;
				default:
					throw new InvalidOperationException();
			}

			return result;
		}
		
		public ISuit Suit { get;  private set; }
		public int Number { get; private set; }
	    private int _value=int.MinValue;
	    public int Value
	    {
	        get
	        {
	            if(_value==int.MinValue)
	            {
	                _value = GetValue(Number);
	            }
                return _value;
	        }
	    }

	    public string ToShortString()
	    {
	        return string.Format("{0}{1}", Suit.Name[0], Number);
	    }

	    public override string ToString()
		{
			return string.Format("{0} - {1}", Number, Suit.Name);
		}

		public override bool Equals(Object obj)
		{
			if (obj == null)
			{
				return false;
			}

			var other = obj as Card;
			if (other == null)
			{
				return false;
			}
			return Equals(other);

		}

		public bool Equals(Card other)
		{
			if ((object)other == null)
			{
				return false;
			}

			return Number == other.Number &&
				string.Compare(Suit.Name, other.Suit.Name, StringComparison.InvariantCultureIgnoreCase) == 0;
		}

		public override int GetHashCode()
		{
			return Number ^ Suit.Name.GetHashCode();
		}

		public static bool operator ==(Card a, Card b)
		{
			if (ReferenceEquals(a, b))
				return true;

			if (((object)a == null) || ((object)b == null))
				return false;

			return a.Equals(b);
		}

		public static bool operator !=(Card a, Card b)
		{
			return !(a == b);
		}

	}
}
