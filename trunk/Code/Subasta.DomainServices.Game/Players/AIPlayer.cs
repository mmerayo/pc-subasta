using System;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Players
{
	internal abstract class AIPlayer : Player
	{
		protected ISimulator Simulator { get; private set; }
		public ISaysSimulator SaysSimulator { get; private set; }
	    protected IFiguresSolver FiguresSolver { get; private set; }

	    protected AIPlayer(ISimulator simulator,ISaysSimulator saysSimulator,IFiguresSolver figuresSolver)
		{
			Simulator = simulator;
			SaysSimulator = saysSimulator;
		    FiguresSolver = figuresSolver;
		}

		public override NodeResult ChooseMove(IExplorationStatus currentStatus)
		{
			return Simulator.GetBest(currentStatus);
		}

		public override Domain.Declaration? ChooseDeclaration(IExplorationStatus previousStatus)
		{
			throw new InvalidOperationException("Not valid YET for AI players. Need to correct the interaction");
		}

		public override SayKind ChooseSay(ISaysStatus saysStatus)
		{
		    return FiguresSolver.GetSay(saysStatus);
		}
	}
}