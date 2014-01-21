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

	class Hand : IHand
	{
		public int Add(int playerPlays, ICard card)
		{
			throw new System.NotImplementedException();
		}

		public bool IsCompleted { get; private set; }
		public int PlayerWinner { get; private set; }
		public int Points { get; private set; }
	}
}