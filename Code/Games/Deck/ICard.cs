namespace Games.Deck
{
	public interface ICard
	{
		ISuit Suit { get; }
		int Number { get; }
	}
}