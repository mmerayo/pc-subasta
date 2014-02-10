using System;
using System.Linq;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.DomainServices;
using Subasta.DomainServices.Game;

namespace Subasta.Infrastructure.DomainServices
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
				Func<ICard, bool> raiseSuit = x => x.Suit.Equals(currentHand.StartedBySuit) && Compare(x,currentHand.CardWinner)==-1;

				//las del palo que suben 
				if (playerCards.Any(raiseSuit)) return playerCards.Where(raiseSuit).ToArray();

				//las del palo
				if (playerCards.Any(currentSuit)) return playerCards.Where(currentSuit).ToArray();

				//todas valen
				return playerCards;
			}

			//no es triunfo

			//esta fallada
			if (currentHand.BrokeToTrump)
			{
				//las del palo
				if (playerCards.Any(currentSuit)) return playerCards.Where(currentSuit).ToArray();

				//subir fallo
				Func<ICard, bool> raiseTrump = x => x.Suit.Equals(currentHand.Trump) && Compare(x, currentHand.CardWinner) == -1;
				if (playerCards.Any(raiseTrump)) return playerCards.Where(raiseTrump).ToArray();

			}

			//no esta fallada
			if (!currentHand.BrokeToTrump)
			{

				Func<ICard, bool> raiseSuit =
					x => x.Suit.Equals(currentHand.StartedBySuit) && Compare(x, currentHand.CardWinner) == -1;
				;
				
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
			
			//todas valen
			return playerCards;
		}

		private int Compare(ICard a, ICard b)
		{
			if(a.Suit!=b.Suit)
				throw new InvalidOperationException();
			if (a.Value > b.Value)
				return -1;
			if (b.Value > a.Value)
				return 1;
			if (a.Number > b.Number)
				return -1;
			return 1;
		}
	}
}