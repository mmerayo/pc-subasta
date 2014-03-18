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

		private static readonly ISuit[] _suits = new ISuit[]
					{
						new Suit("Oros", 1),
						new Suit("Copas", 2),
						new Suit("Espadas", 3),
						new Suit("Bastos", 4)
					};


		public static ISuit FromName(string suitName)
		{
			return Suits.Single(x => string.Compare(x.Name, suitName, StringComparison.InvariantCultureIgnoreCase) == 0);
		}

		public static ISuit[] Suits
		{
			get
			{
				return _suits;
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
	                return _suits[0];

                case 'C':
					return _suits[1];

                case 'E':
					return _suits[2];

                case 'B':
					return _suits[3];

	            default:
	                throw new ArgumentOutOfRangeException("idName");
	        }
	    }
	}
}