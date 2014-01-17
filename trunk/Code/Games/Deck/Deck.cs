using System.Collections.Generic;

namespace Games.Deck
{
	abstract class Deck : IDeck
	{
		public IPack Cards { get; private set; }
		public void SetCards(IEnumerable<ICard> cards)
		{
			Cards=new Pack(cards);
		}

		protected Deck(IPack cards)
		{
			Cards = cards;
		}
	}
}