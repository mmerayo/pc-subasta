namespace Subasta.Domain.Deck
{
	public interface ICard
	{
		ISuit Suit { get; }
		int Number { get; }
		int Value { get; }
	    string ToShortString();
	}
}