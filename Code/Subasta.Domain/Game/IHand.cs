using System.Collections;
using System.Collections.Generic;
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
	    byte Add(byte playerPlays, ICard candidate);
		bool IsCompleted { get; }
		byte? PlayerWinner { get; }
		byte Points { get; }
		bool IsStartedByTrump { get; }
		ISuit StartedBySuit { get; }
		ICard CardWinner { get; }
		bool IsEmpty { get; }
		byte? TeamWinner { get; }
		
		/// <summary>
		/// fallada
		/// </summary>
		bool BrokeToTrump { get; }

		ISuit Trump { get; }
		Declaration? Declaration { get; }
		byte FirstPlayer { get; }
		byte LastPlayer { get; }

		byte Sequence { get; }
		byte DeclarationValue { get; }
		byte LastPlayerPlayed { get; }
    	byte NumberCardsPlayed { get; }
    	ICard PlayerCardResolve(int playerPosition);
		byte PlayerCardResolve(ICard card);

		IHand Clone(IExplorationStatus container);
		void SetDeclaration(Declaration? declaration);
	    IEnumerable<ICard> CardsByPlaySequence();
	}
	
}