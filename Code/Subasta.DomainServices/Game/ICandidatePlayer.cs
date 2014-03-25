using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game
{
	public interface ICandidatePlayer
	{
		IExplorationStatus PlayCandidate(IExplorationStatus currentStatus, int playerPosition, ICard candidate);//TODO: REMOVE PLAYER POSITION AS IS ALWAYS THE TURN
	}
}