using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game
{
	public interface ICandidatesSelector
	{
		ICard[] GetCandidates(IExplorationStatus currentStatus, int playerPosition);
	}
}