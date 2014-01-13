namespace Games.Deck
{
	internal class Suit : ISuit
	{
		public Suit(string name,int value)
		{
			Name = name;
			Value = value;
		}

		public string Name { get; private set; }
		public int Value { get; private set; }
	}
}