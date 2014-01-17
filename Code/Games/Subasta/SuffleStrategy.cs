using System;
using System.Collections.Generic;
using System.Linq;
using Games.Deck;

namespace Games.Subasta
{
	class SuffleStrategy : ISuffleStrategy
	{
		public void Suffle(ref IDeck deck)
		{
			var cards = deck.Cards.ToList();

			var numExchanges= GetRandomInt(10,40);
			var count = cards.Count();
			while (numExchanges-- > 0)
			{
				
				var index = GetRandomInt(0, count - 1);
				var index2 = GetRandomInt(0, count - 1);

				if (index != index2)
				{
					var tmp = cards[index2];
					cards[index2] = cards[index];
					cards[index] = tmp;
				}

			}

			deck.SetCards(cards);

		}

		private int GetRandomInt(int min, int max)
		{
			return new Random((int)DateTime.Now.Ticks).Next(min,max);
		}
	}
}