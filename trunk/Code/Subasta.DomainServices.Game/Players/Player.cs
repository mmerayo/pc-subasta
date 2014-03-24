using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Players
{
	internal abstract class Player:IPlayer
	{
		protected ISimulator Simulator { get; private set; }

		protected Player(ISimulator simulator)
		{
			Simulator = simulator;
		}


		public ICard[] Cards { get; set; }
		public string Name { get; set; }
		public int TeamNumber { get; set; }
		public abstract void SetNewGame(IExplorationStatus initialStatus);
		public NodeResult ChooseMove(IExplorationStatus currentStatus)
		{
			return Simulator.GetBest(currentStatus);
		}

		public void Stop()
		{
			Simulator.Stop();
		}
	}

	
}
