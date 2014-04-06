using Subasta.Domain.DalModels;
using Subasta.Domain.Game;
using Subasta.DomainServices.Game.Algorithms.MCTS;

namespace Subasta.DomainServices.Game.Players
{
	internal class MctsPlayer:AIPlayer,IMctsPlayer
	{

		public MctsPlayer(IMctsRunner simulator, IMctsSaysRunner saysSimulator)
			: base(simulator,saysSimulator)
		{
		}

		public override PlayerType PlayerType
		{
			get { return PlayerType.Mcts; }
		}


	    public override void Reset()
	    {
	        Simulator.Reset();
	        base.Reset();
	    }
	}
} 
  