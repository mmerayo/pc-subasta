using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.MCTS
{
	internal interface IMctsRunner:ISimulator
	{

		void Pause();
		void Restart();
		
	}
}