using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Players
{
	internal class HumanPlayer : Player, IHumanPlayer
	{
		public override NodeResult ChooseMove(IExplorationStatus currentStatus)
		{
			throw new System.NotImplementedException();
		}
	}
}