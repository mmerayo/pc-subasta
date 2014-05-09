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
		public byte TeamNumber { get; set; }
		public byte PlayerNumber { get; set; }
		public abstract NodeResult ChooseMove(IExplorationStatus currentStatus, out bool peta);
		public abstract Declaration? ChooseDeclaration(IExplorationStatus previousStatus);
	    public virtual void Reset(){}
		public abstract IFigure ChooseSay(ISaysStatus saysStatus);
		public abstract ISuit ChooseTrump(ISaysStatus saysStatus);
		public int NextNumber()
		{
			if (PlayerNumber == 4)
				return 1;
			return PlayerNumber + 1;
		}
	}
}
