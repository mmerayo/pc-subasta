using Subasta.Domain.Deck;

namespace Subasta.DomainServices.Game.Utils
{
	class CardComparer : ICardComparer
	{
		public ICard Best(ISuit trump, ICard current, ICard candidate)
		{
			//candidato es triunfo
			if (candidate.Suit.IsTrump(trump))
			{
				//el existente no es triunfo
				if (!current.Suit.IsTrump(trump))
					return candidate;
				//son triunfos los 2
				if (current.Value > candidate.Value || (current.Value==candidate.Value && current.Number>candidate.Number))
					return current;
				return candidate;
			}
			//candidato no es triunfo y el current lo es
			if (current.Suit.IsTrump(trump))
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