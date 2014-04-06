using Subasta.Domain;
using Subasta.Domain.DalModels;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Players
{
	internal abstract class Player:IPlayer
	{
		public ICard[] Cards { get; set; }
		public abstract PlayerType PlayerType { get; }
		public string Name { get; set; }
		public int TeamNumber { get; set; }
		public int PlayerNumber { get; set; }
		public abstract NodeResult ChooseMove(IExplorationStatus currentStatus);
		public abstract Declaration? ChooseDeclaration(IExplorationStatus previousStatus);
	    public virtual void Reset(){}
		public abstract SayKind ChooseSay(ISaysStatus saysStatus);
	}
}
