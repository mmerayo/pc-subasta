using Subasta.Domain.DalModels;
using Subasta.Domain.Game;
using Subasta.DomainServices.Game.Algorithms.MCTS;

namespace Subasta.DomainServices.Game.Players
{
	internal class MctsPlayer:AIPlayer,IMctsPlayer
	{

		public MctsPlayer(IMctsRunner simulator):base(simulator)
		{
		}

		public override PlayerType PlayerType
		{
			get { return PlayerType.Mcts; }
		}

		public override void SetNewGame(IExplorationStatus initialStatus)
		{
			Simulator.Start(TeamNumber,initialStatus);
		}
	}
}