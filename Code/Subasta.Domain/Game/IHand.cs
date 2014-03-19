using Subasta.Domain.Deck;

namespace Subasta.Domain.Game
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
		int? PlayerWinner { get; }
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
        int FirstPlayer { get; }
        int Sequence { get; }
        ICard PlayerCard(int playerPosition);

		IHand Clone();
		void SetDeclaration(Declaration declaration);
	}
}