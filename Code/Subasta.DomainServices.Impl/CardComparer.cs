namespace Subasta.DomainServices.Impl
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
			//candidato es triunfo
			if (candidate.Suit.IsTrump(_trump))
			{
				//el existente no es triunfo
				if (!current.Suit.IsTrump(_trump))
					return candidate;
				//son triunfos los 2
				if (current.Value > candidate.Value || (current.Value==candidate.Value && current.Number>candidate.Number))
					return current;
				return candidate;
			}
			//candidato no es triunfo y el current lo es
			if (current.Suit.IsTrump(_trump))
				return current;

			//son del mismo palo
			if (candidate.Suit==current.Suit &&
				( candidate.Value > current.Value
				|| (current.Value == candidate.Value && candidate.Number > current.Number)))
				return candidate;
			return current;
		}
	}
}