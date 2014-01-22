using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Games.Deck;

namespace Games.Subasta
{
	interface ICardComparer
	{
		ICard Best(ICard a, ICard b);
	}
}
