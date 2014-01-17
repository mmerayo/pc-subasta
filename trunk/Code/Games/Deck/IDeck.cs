using System.Collections.Generic;

namespace Games.Deck
{
	public interface IDeck
	{
		IPack Cards { get; }
		void SetCards(IEnumerable<ICard> cards);
	}
}
