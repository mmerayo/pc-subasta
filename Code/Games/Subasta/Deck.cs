﻿using System.Collections.Generic;
using Games.Deck;

namespace Games.Subasta
{
	class Deck : Games.Deck.Deck
	{
		public Deck():base(CreateCards())
		{
		}

		protected static IPack CreateCards()
		{
			var result = new Pack();
			var suits= Suit.Suits;
			foreach (var suit in suits)
			{
				result.AddRange(GetSuitCards(suit));
			}

			return result;
		}

		private static IEnumerable<ICard> GetSuitCards(ISuit suit)
		{
			var result = new List<ICard>
				{
					new Card(suit, 1),
					new Card(suit, 2),
					new Card(suit, 3),
					new Card(suit, 4),
					new Card(suit, 5),
					new Card(suit, 6),
					new Card(suit, 7),
					new Card(suit, 10),
					new Card(suit, 11),
					new Card(suit, 12),
				};
			return result;
		}
	}
}