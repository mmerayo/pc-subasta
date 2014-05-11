using Subasta.Domain.Deck;

namespace Subasta.DomainServices.Game
{
	public interface IDeckShuffler
	{
		IDeck Shuffle(IDeck cardsSource);
	}
}