using Games.Deck;
using Games.Subasta.GameGeneration.AI;

namespace Games.Subasta
{

	internal interface IHand
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
		bool IsStartedByTrump { get; }
		ISuit StartedBySuit { get; }
		ICard CardWinner { get; }
		bool IsEmpty { get; }
		
		
		/// <summary>
		/// fallada
		/// </summary>
		bool BrokeToTrump { get; }

		ISuit Trump { get; }
		Declaration? Declaration { get; }
		ICard PlayerCard(int playerPosition);

		IHand Clone();
		void Add(Declaration declaration);
	}
}