using Subasta.Domain.Deck;

namespace Subasta.DomainServices.Game
{
	public interface IDeckSuffler
	{
		IDeck Suffle(IDeck cardsSource);
	}
}