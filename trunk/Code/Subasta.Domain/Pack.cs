using System.Collections.Generic;
using System.Linq;

namespace Games.Deck
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