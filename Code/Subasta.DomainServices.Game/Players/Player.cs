using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Players
{
	internal abstract class Player:IPlayer
	{
		public ICard[] Cards { get; set; }
		public string Name { get; set; }
		public int TeamNumber { get; set; }
		public abstract void SetNewGame(IExplorationStatus initialStatus);
		public abstract NodeResult ChooseMove(IExplorationStatus currentStatus);
		public virtual void Stop()
		{
			
		}
	}
}
