using System.Collections.Generic;

namespace Games.Deck
{
	public interface IPack
	{
		IEnumerable<ICard> ToList();
	}
}