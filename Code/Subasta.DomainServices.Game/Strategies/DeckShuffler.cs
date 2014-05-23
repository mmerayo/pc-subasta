using System;
using System.Collections.Generic;
using System.Diagnostics;
using Subasta.Domain.Deck;

namespace Subasta.DomainServices.Game.Strategies
{
	internal class DeckShuffler:IDeckShuffler
	{
		private static readonly Random Rnd = new Random((int)DateTime.UtcNow.Ticks);
		private int _times = FirstTimeTimes;
		private const int FirstTimeTimes = 5;

		public IDeck Shuffle(IDeck deck)
		{
			var cards = deck.Cards.Cards;

			for (int i = 0; i < _times; i++)
			{
				var idx = GetRandomIndex(deck);
				var lenght = GetRandomLength(cards.Count - idx - 1);

				DoShuffle(idx, lenght, cards);
			}

			if (_times == FirstTimeTimes)
				_times = 3;

			return deck;
		}

		private static void DoShuffle(int idx,int lenght, List<ICard> cards)
		{
			Debug.WriteLine("DoShuffle - idx:{0}, lenght:{1}", idx, lenght);
			var toMove = cards.GetRange(idx, lenght);
			cards.RemoveRange(idx, lenght);
			cards.AddRange(toMove);
		}

		private static int GetRandomLength(int maxLenght)
		{
			return Rnd.Next(1, maxLenght);
		}

		private static int GetRandomIndex(IDeck deck)
		{
			return Rnd.Next(0, deck.Cards.Cards.Count - 2);
		}
	}
}