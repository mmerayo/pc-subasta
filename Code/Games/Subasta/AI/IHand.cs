using Games.Deck;

namespace Games.Subasta.AI
{
	public interface IHand
	{
		void Add(int playerPlays, ICard candidate);
		bool IsCompleted { get; }
		int PlayerWinner { get; }
	}
}