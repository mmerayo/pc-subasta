namespace Games.Deck
{
	public interface ISuit
	{
		string Name { get; }
		int Value { get; }
		bool IsTrump(ISuit Trump);
	}
}