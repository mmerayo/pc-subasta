using Games.Deck;

namespace Games.Subasta
{
	class CardComparer : ICardComparer
	{
		private readonly ISuit _trump;

		public CardComparer(ISuit trump)
		{
			_trump = trump;
		}

		public ICard Best(ICard a, ICard b)
		{
			if (b.Suit.Leads(_trump))
			{
				if (!a.Suit.Leads(_trump))
					return b;
				if (a.Value > b.Value)
					return a;
				return b;
			}

			if (a.Suit.Leads(_trump))
				return a;

			if (a.Value > b.Value)
				return a;
			return b;
		}
	}
}