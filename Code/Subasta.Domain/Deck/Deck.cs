using System;
using System.Collections.Generic;
using System.Linq;

namespace Subasta.Domain.Deck
{
	abstract class Deck : IDeck
	{
		public IPack Cards { get; private set; }
		public void SetCards(IPack cards)
		{
			Cards=cards;
		}

		public ICard Get(int number, string suitName)
		{
			return
				Cards.Cards
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