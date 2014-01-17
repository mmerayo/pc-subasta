using System.Collections.Generic;

namespace Games.Deck
{
	public interface IPack
	{
		List<ICard> ToList();
	}
}