using System.Collections.Generic;

namespace Games.Deck.Spanish
{
	class Deck : Games.Deck.Deck
	{
		public Deck():base(CreateCards())
		{
		}

		protected static IPack CreateCards()
		{
			var result = new Pack();
			var suits= CreateSuits();
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
					new Subasta.Card(suit, 1),
					new Subasta.Card(suit, 2),
					new Subasta.Card(suit, 3),
					new Subasta.Card(suit, 4),
					new Subasta.Card(suit, 5),
					new Subasta.Card(suit, 6),
					new Subasta.Card(suit, 7),
					new Subasta.Card(suit, 10),
					new Subasta.Card(suit, 11),
					new Subasta.Card(suit, 12),
				};
			return result;
		}

		private static IEnumerable<ISuit> CreateSuits()
		{
			var result = new List<ISuit>
				{
					new Suit("Oros", 1),
					new Suit("Copas", 2),
					new Suit("Espadas", 3),
					new Suit("Bastos", 4)
				};
			return result;
		}
	}
}