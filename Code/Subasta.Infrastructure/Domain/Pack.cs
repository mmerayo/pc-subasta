﻿using System.Collections.Generic;
using System.Linq;
using Subasta.Domain.Deck;

namespace Subasta.Infrastructure.Domain
{
	class Pack : IPack
	{
		private readonly List<ICard> _cards;

		public Pack():this(new List<ICard>())
		{
		}
		public Pack(IEnumerable<ICard> cards)
		{
			_cards = cards.ToList();
		}

		public List<ICard> Cards
		{
			get { return _cards; }
		}

		public void AddRange(IEnumerable<ICard> items)
		{
			Cards.AddRange(items);
		}
	}
}