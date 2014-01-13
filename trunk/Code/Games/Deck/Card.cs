namespace Games.Deck
{
	public abstract class Card
	{
		internal Card(Suit suit, int number)
		{
			Suit = suit;
			Number = number;
			
		}

		
		public Suit Suit { get;  private set; }
		public int Number { get; private set; }
	}
}
