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

		byte GetMaxExplorationFor(byte teamNumber, int minNumberExplorations, float maxRiskPercentage);
		void UpdateExplorationListeners();
	}
}