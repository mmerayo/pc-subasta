using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Players
{
	internal abstract class AIPlayer: Player
	{
		protected ISimulator Simulator { get; private set; }

		protected AIPlayer(ISimulator simulator)
		{
			Simulator = simulator;
			simulator.Player = this;
		}

		public override NodeResult ChooseMove(IExplorationStatus currentStatus)
		{
			return Simulator.GetBest(currentStatus);
		}
	}
}