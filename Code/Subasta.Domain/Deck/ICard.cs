namespace Subasta.Domain.Deck
{
	public interface ICard
	{
		ISuit Suit { get; }
		byte Number { get; }
		byte Value { get; }
	    string ToShortString();
		bool IsAbsSmallerThan(ICard card);
	}
}