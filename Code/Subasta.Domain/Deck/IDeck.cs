using System.Collections.Generic;

namespace Subasta.Domain.Deck
{
	public interface IDeck
	{
		IPack Cards { get; }
		void SetCards(IPack cards);
		ICard Get(int number, string suitName);
	}
}
