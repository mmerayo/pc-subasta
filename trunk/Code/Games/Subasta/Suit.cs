using System.Collections.Generic;
using Games.Deck;

namespace Games.Subasta
{
	class Suit:Games.Deck.Suit
	{
		private static ISuit[] _suits;

		public Suit(string name, int value) : base(name, value)
		{
		}

		public static IEnumerable<ISuit> GetSuits()
		{
			return _suits ?? (_suits = new  ISuit[]
				{
					new Suit("Oros", 1),
					new Suit("Copas", 2),
					new Suit("Espadas", 3),
					new Suit("Bastos", 4)
				});
		}
	}
}