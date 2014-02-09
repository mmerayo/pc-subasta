using Subasta.Domain.Game;

namespace Subasta.ApplicationServices
{
	public interface IGameExplorer
	{
		NodeResult Execute(IExplorationStatus currentStatus, int playerPosition);
	}
}