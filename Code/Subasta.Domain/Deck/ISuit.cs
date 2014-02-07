namespace Subasta.Domain.Deck
{
	public interface ISuit
	{
		string Name { get; }
		int Value { get; }
		bool IsTrump(ISuit trump);
	}
}