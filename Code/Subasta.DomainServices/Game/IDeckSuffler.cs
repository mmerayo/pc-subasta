using Subasta.Domain.Deck;

namespace Subasta.DomainServices.Game
{
	public interface IDeckSuffler
	{
		IPack Suffle(IPack cardsSource);
	}
}