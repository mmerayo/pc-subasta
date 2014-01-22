using Games.Deck;

namespace Games.Subasta
{
	class CardComparer : ICardComparer
	{
		private readonly ISuit _Trump;

		public CardComparer(ISuit Trump)
		{
			_Trump = Trump;
		}

		public ICard Best(ICard a, ICard b)
		{
			if (b.Suit.Leads(_Trump))
			{
				if (!a.Suit.Leads(_Trump))
					return b;
				if (a.Value > b.Value)
					return a;
				return b;
			}

			if (a.Suit.Leads(_Trump))
				return a;

			if (a.Value > b.Value)
				return a;
			return b;
		}
	}
}