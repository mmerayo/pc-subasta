using System;
using System.Collections.Generic;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.MCTS
{
	internal class FiguresSolver : IFiguresSolver
	{
		private readonly ISaysSimulator _saysSimulator;

		public FiguresSolver(ISaysSimulator saysSimulator)
		{
			_saysSimulator = saysSimulator;
		}

		public SayKind GetSay(ISaysStatus saysStatus)
		{
			var candidates=GetCandidateFigures();

			var maxCurrentExploration = GetMaxCurrentExploration(saysStatus.TurnTeam);
			FilterPassedCandidateFigures(candidates,maxCurrentExploration,saysStatus);
			FilterNonReachableCandidateFigures(candidates,maxCurrentExploration,saysStatus);
			return Resolve(candidates, maxCurrentExploration,saysStatus);

		}

		private void FilterNonReachableCandidateFigures(List<SayKind> candidates, int maxCurrentExploration, ISaysStatus saysStatus)
		{
			//filter those figures that cannot be reached due to limit in the exploration
			throw new NotImplementedException();
		}

		private void FilterPassedCandidateFigures(List<SayKind> candidates, int maxCurrentExploration, ISaysStatus saysStatus)
		{
			//filter those figures that cannot be declared again due to limit in the exploration
			//when a figure wasnt said and cannot be said porque se ha superado(Ases y parejas), mark items of UnaMas

			throw new NotImplementedException();
		}

		private int GetMaxCurrentExploration(int turnTeam)
		{
			return _saysSimulator.GetMaxExplorationFor(turnTeam);
		}

		private SayKind Resolve(List<SayKind> candidates, int topPoints,ISaysStatus saysStatus)
		{
			//TODO:unit test
			//TODO:move THIS FEATURE to a dedicated class
			throw new NotImplementedException();
			//if the team mate passed try to close or all the figures have been said
			//check available figures and chose the one with the lowest value
			//if there are not figures then try to close if the player cards value in the siut is higher than the mate ones
		}

		private List<SayKind> GetCandidateFigures()
		{
			throw new NotImplementedException();
		}

	}
}