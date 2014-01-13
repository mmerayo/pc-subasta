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

		public void AddRange(IEnumerable<ICard> items)
		{
			_cards.AddRange(items);
		}

		public IEnumerable<ICard> ToList()
		{
			return _cards;
		}
	}
}