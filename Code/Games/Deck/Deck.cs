using System.Collections.Generic;

namespace Games.Deck
{
	abstract class Deck : IDeck
	{
		public IPack Cards { get; private set; }

		protected Deck(IPack cards)
		{
			Cards = cards;
		}
	}
}