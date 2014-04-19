using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game
{
	public interface ISaysSimulator
	{
		void Start(ISaysStatus sourceStatus);
		void Reset(object result);
		ISuit ChooseTrump(int teamNumber);
		object GetRoot(ISuit chooseTrump); //TODO: TYPE

		int GetMaxExplorationFor(int teamNumber, int minNumberExplorations, double maxRiskPercentage);
		void UpdateExplorationListeners();
	}
}