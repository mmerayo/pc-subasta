using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Subasta.Domain.DalModels;
using Subasta.Domain.Deck;

namespace Subasta.Domain.Game
{
	public interface IPlayer
	{
		ICard[] Cards { get; set; }
		PlayerType PlayerType { get; }
		string Name { get; set; }
		byte TeamNumber { get; set; }
		byte PlayerNumber { get; set; }
		NodeResult ChooseMove(IExplorationStatus currentStatus,out bool peta);

		Declaration? ChooseDeclaration(IExplorationStatus previousStatus);
	    void Reset();
		IFigure ChooseSay(ISaysStatus saysStatus);
		ISuit ChooseTrump(ISaysStatus saysStatus);
	}
}
