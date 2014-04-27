using System;
using System.Collections.Generic;
using System.Diagnostics;
using Subasta.Domain.Deck;

namespace Subasta.DomainServices.Game.Strategies
{
	internal class DeckSuffler:IDeckSuffler
	{
		private static Random _rnd = new Random((int)DateTime.UtcNow.Ticks);

		public IDeck Suffle(IDeck deck)
		{
			var cards = deck.Cards.Cards;

			for (int i = 0; i < 10; i++)
			{
				var idx = GetRandomIndex(deck);
				var lenght = GetRandomLength(cards.Count - idx - 1);

				DoSuffle(idx, lenght, cards);
			}
			return deck;
		}

		private static void DoSuffle(int idx,int lenght, List<ICard> cards)
		{
			Debug.WriteLine("Dosuffle - idx:{0}, lenght:{1}", idx, lenght);
			var toMove = cards.GetRange(idx, lenght);
			cards.RemoveRange(idx, lenght);
			cards.AddRange(toMove);
		}

		private static int GetRandomLength(int maxLenght)
		{
			return _rnd.Next(1, maxLenght);
		}

		private static int GetRandomIndex(IDeck deck)
		{
			return _rnd.Next(0, deck.Cards.Cards.Count - 2);
		}
	}
}