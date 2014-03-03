using System;
using System.Collections.Generic;
using System.Linq;
using Subasta.Domain.Deck;

namespace Subasta.Infrastructure.Domain
{
	public sealed class Suit : ISuit
	{
		public Suit(string name,int value)
		{
			Name = name;
			Value = value;
		}

		public string Name { get; private set; }

		public int Value { get; private set; }

		public override string ToString()
		{
			return Name;
		}

		private static ISuit[] _suits;


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

		public  bool IsTrump(ISuit trump)
		{
			return Value == trump.Value;
		}

	    public static ISuit FromId(char idName)
	    {
	        switch (Char.ToUpper(idName))
	        {
                case 'O':
	                return FromName("Oros");

                case 'C':
                    return FromName("Oros");

                case 'E':
                    return FromName("Oros");

                case 'B':
                    return FromName("Oros");

	            default:
	                throw new ArgumentOutOfRangeException("idName");
	        }
	    }
	}
}