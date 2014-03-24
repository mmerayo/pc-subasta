using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Subasta.Domain.Deck;

namespace Subasta.Domain.Game
{
	public interface IPlayer
	{
		ICard[] Cards { get; set; }

		string Name { get; set; }
		int TeamNumber { get; set; }
		void SetNewGame(IExplorationStatus initialStatus);
		NodeResult ChooseMove(IExplorationStatus currentStatus);
		void Stop();
	}
}
