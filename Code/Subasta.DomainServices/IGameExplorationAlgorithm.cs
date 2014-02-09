using Subasta.Domain.Game;

namespace Subasta.DomainServices
{
	public interface IGameExplorationAlgorithm
	{
		NodeResult Execute(IExplorationStatus currentStatus, int playerPosition);
	}
}