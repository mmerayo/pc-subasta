using System;
using System.Linq;
using Games.Deck;

namespace Games.Subasta
{
	class ValidCardsRule : IValidCardsRule
	{

		public ICard[] GetValidMoves(ICard[] playerCards, IHand currentHand)
		{
			//primero todas valen
			if (currentHand.IsEmpty)
				return playerCards;
				
			Func<ICard, bool> currentSuit = x => x.Suit.Equals(currentHand.StartedBySuit);

			//es triunfo 
			if (currentHand.IsStartedByTrump)
			{
				Func<ICard, bool> raiseSuit = x => x.Suit.Equals(currentHand.StartedBySuit) && x.Value > currentHand.CardWinner.Value;

				//las del palo que suben 
				if (playerCards.Any(raiseSuit)) return playerCards.Where(raiseSuit).ToArray();

				//las del palo
				if (playerCards.Any(currentSuit)) return playerCards.Where(currentSuit).ToArray();

				//todas valen
				return playerCards;
			}

			//no es triunfo

			//no esta fallada
			if (!currentHand.BrokeToTrump)
			{
				
				Func<ICard, bool> raiseSuit = x => x.Suit.Equals(currentHand.StartedBySuit) && x.Value > currentHand.CardWinner.Value;
				
				//las del palo que suben 
				if (playerCards.Any(raiseSuit)) return playerCards.Where(raiseSuit).ToArray();

				//las del palo
				if (playerCards.Any(currentSuit)) return playerCards.Where(currentSuit).ToArray();

				//las de triunfo
				Func<ICard, bool> allTrumpCards = x => x.Suit.Equals(currentHand.Trump);
				if (playerCards.Any(allTrumpCards)) return playerCards.Where(allTrumpCards).ToArray();

				//todas valen
				return playerCards;

			}
			
			//esta fallada

			//las del palo
			if (playerCards.Any(currentSuit)) return playerCards.Where(currentSuit).ToArray();

			//subir fallo
			Func<ICard, bool> raiseTrump = x => x.Suit.Equals(currentHand.Trump) && x.Value > currentHand.CardWinner.Value;
			if (playerCards.Any(raiseTrump)) return playerCards.Where(raiseTrump).ToArray();
			
			//todas valen
			return playerCards;
		}
	}
}