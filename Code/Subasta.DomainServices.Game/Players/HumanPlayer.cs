using Subasta.Domain.Game;
using Subasta.DomainServices.Game.Algorithms.MaxN;

namespace Subasta.DomainServices.Game.Players
{
	internal class HumanPlayer : AIPlayer, IHumanPlayer
	{
		public MaxNPlayer(IMaxNSimulator simulator)
			: base(simulator)
		{

		}

		public override void SetNewGame(IExplorationStatus initialStatus)
		{
			//TODO:?
		}
	}
}