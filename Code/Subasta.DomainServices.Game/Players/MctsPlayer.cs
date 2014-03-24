using Subasta.Domain.Game;
using Subasta.DomainServices.Game.Algorithms.MCTS;

namespace Subasta.DomainServices.Game.Players
{
	internal class MctsPlayer:Player,IMctsPlayer
	{

		public MctsPlayer(IMctsRunner simulator):base(simulator)
		{
		}

		public override void SetNewGame(IExplorationStatus initialStatus)
		{
			Simulator.Stop();
			Simulator.Start(TeamNumber,initialStatus);
		}
	}
}