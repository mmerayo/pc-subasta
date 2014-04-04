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
		int TeamNumber { get; set; }
		int PlayerNumber { get; set; }
		NodeResult ChooseMove(IExplorationStatus currentStatus);

		Declaration? ChooseDeclaration(IExplorationStatus previousStatus);
	    void Reset();
	}
}
