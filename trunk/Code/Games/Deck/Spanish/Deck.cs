using System.Collections.Generic;

namespace Games.Deck.Spanish
{
	class Deck : Games.Deck.Deck
	{
		public Deck():base(CreateCards())
		{
		}

		private static IEnumerable<ICard> CreateCards()
		{
			throw new System.NotImplementedException();
		}
	}
}