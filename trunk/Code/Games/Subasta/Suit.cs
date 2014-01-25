using System;
using System.Collections.Generic;
using System.Linq;
using Games.Deck;

namespace Games.Subasta
{
	internal class Suit : Games.Deck.Suit
	{
		private static ISuit[] _suits;

		public Suit(string name, int value)
			: base(name, value)
		{
		}

		public static ISuit FromName(string suitName)
		{
			return Suits.Single(x => string.Compare(x.Name, suitName, StringComparison.InvariantCultureIgnoreCase) == 0);
		}

		public static IEnumerable<ISuit> Suits
		{
			get
			{
				return _suits ?? (_suits = new ISuit[]
					{
						new Suit("Oros", 1),
						new Suit("Copas", 2),
						new Suit("Espadas", 3),
						new Suit("Bastos", 4)
					});
			}
		}

		public override bool IsTrump(ISuit Trump)
		{
			return Value == Trump.Value;
		}

		
	}
}