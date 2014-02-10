using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game
{
	public interface IGameExplorer
	{
		NodeResult Execute(IExplorationStatus currentStatus, int playerPosition);
	}
}