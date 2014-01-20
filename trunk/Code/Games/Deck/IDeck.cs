using System.Collections.Generic;

namespace Games.Deck
{
	public interface IDeck
	{
		IPack Cards { get; }
		void SetCards(IEnumerable<ICard> cards);
		ICard Get(int number, string suitName);
	}
}
