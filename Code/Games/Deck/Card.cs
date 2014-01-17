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
	}
}
