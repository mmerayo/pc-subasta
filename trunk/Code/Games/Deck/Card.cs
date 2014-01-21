using System;

namespace Games.Deck
{
	public abstract class Card : ICard
	{
		internal Card(ISuit suit, int number)
		{
			Suit = suit;
			Number = number;
		}
		
		public ISuit Suit { get;  private set; }
		public int Number { get; private set; }
		public int Value { get; protected set; }

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

		public virtual bool Equals(Card other)
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
