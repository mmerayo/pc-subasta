using System;
using Subasta.Domain.Deck;

namespace Subasta.DomainServices.Game.Strategies
{
	internal class DeckSuffler:IDeckSuffler
	{
		public IDeck Suffle(IDeck deck)
		{
			var cards = deck.Cards.Cards;

			for (int i = 0; i < 8; i++)
			{
				var idx = GetRandomIndex(deck);
				var lenght = GetRandomLength(cards.Count - idx - 1);

				var toMove = cards.GetRange(idx, lenght);
				cards.RemoveRange(idx, lenght);
				cards.AddRange(toMove);
			}
			return deck;
		}

		private static int GetRandomLength(int maxLenght)
		{
			return new Random((int)DateTime.UtcNow.Ticks).Next(1, maxLenght);
		}

		private static int GetRandomIndex(IDeck deck)
		{
			return new Random((int)DateTime.UtcNow.Ticks).Next(0, deck.Cards.Cards.Count - 2);
		}
	}
}