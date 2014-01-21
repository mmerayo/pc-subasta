using Games.Deck;

namespace Games.Subasta
{
	public interface IHand
	{
		/// <summary>
		/// Adds a card
		/// </summary>
		/// <param name="playerPlays"></param>
		/// <param name="candidate"></param>
		/// <returns> the position of the card in the hand</returns>
		int Add(int playerPlays, ICard candidate);
		bool IsCompleted { get; }
		int PlayerWinner { get; }
		int Points { get; }
	}
}