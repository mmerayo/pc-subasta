namespace Games.Deck
{
	internal abstract class Suit : ISuit
	{
		protected Suit(string name,int value)
		{
			Name = name;
			Value = value;
		}

		public string Name { get; private set; }
		public abstract bool IsTrump(ISuit Trump);

		public int Value { get; private set; }

		public override string ToString()
		{
			return Name;
		}
	}
}