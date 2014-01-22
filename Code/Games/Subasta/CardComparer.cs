using Games.Deck;

namespace Games.Subasta
{
	class CardComparer : ICardComparer
	{
		private readonly ISuit _leadSuit;

		public CardComparer(ISuit leadSuit)
		{
			_leadSuit = leadSuit;
		}

		public ICard Best(ICard a, ICard b)
		{
			if (b.Suit.Leads(_leadSuit))
			{
				if (!a.Suit.Leads(_leadSuit))
					return b;
				if (a.Value > b.Value)
					return a;
				return b;
			}

			if (a.Suit.Leads(_leadSuit))
				return a;

			if (a.Value > b.Value)
				return a;
			return b;
		}
	}
}