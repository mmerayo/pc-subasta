using System;
using System.Collections.Generic;
using System.Linq;

namespace Games.Deck
{
	abstract class Deck : IDeck
	{
		public IPack Cards { get; private set; }
		public void SetCards(IEnumerable<ICard> cards)
		{
			Cards=new Pack(cards);
		}

		public ICard Get(int number, string suitName)
		{
			return
				Cards.ToList()
				     .SingleOrDefault(
					     x =>
					     x.Number == number && string.Compare(x.Suit.Name, suitName, StringComparison.InvariantCultureIgnoreCase) == 0);
		}

		protected Deck(IPack cards)
		{
			Cards = cards;
		}
	}
}