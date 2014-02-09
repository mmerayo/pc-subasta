using Subasta.Domain.Game;

namespace Subasta.DomainServices
{
	public interface IGameExplorer
	{
		NodeResult Execute(IExplorationStatus currentStatus, int playerPosition);
	}
}