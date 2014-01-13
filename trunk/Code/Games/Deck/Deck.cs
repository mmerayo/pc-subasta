using System.Collections.Generic;

namespace Games.Deck
{
	abstract class Deck : IDeck
	{
		public IEnumerable<ICard> Cards { get; private set; }

		protected Deck(IEnumerable<ICard> cards)
		{
			Cards = cards;
		}
	}
}