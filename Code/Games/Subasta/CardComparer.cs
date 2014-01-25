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

		public ICard Best(ICard current, ICard candidate)
		{
			if (candidate.Suit.IsTrump(_trump))
			{
				if (!current.Suit.IsTrump(_trump))
					return candidate;
				if (current.Value > candidate.Value)
					return current;
				return candidate;
			}

			if (current.Suit.IsTrump(_trump))
				return current;

			if (!candidate.Suit.IsTrump(_trump) || current.Value > candidate.Value)
				return current;
			return candidate;
		}
	}
}