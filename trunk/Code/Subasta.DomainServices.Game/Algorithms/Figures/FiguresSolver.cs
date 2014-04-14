using System;
using System.Collections.Generic;
using System.Linq;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures
{
	internal class FiguresSolver : IFiguresSolver
	{
		private readonly ISaysSimulator _saysSimulator;
		private readonly IEnumerable<IFigure> _figures;

		public FiguresSolver(ISaysSimulator saysSimulator,IEnumerable<IFigure> figures )
		{
			_saysSimulator = saysSimulator;
			_figures = figures;
		}

		public IFigure GetFigure(ISaysStatus saysStatus)
		{
			var maxCurrentExploration = GetMaxCurrentExploration(saysStatus.TurnTeam,saysStatus.Says.Count(x=>x.PlayerNum==saysStatus.Turn)*3000);

			var candidates=GetCandidateFigures(saysStatus,maxCurrentExploration);
			
			return Resolve(candidates, maxCurrentExploration,saysStatus);
		 }
		
		private int GetMaxCurrentExploration(int turnTeam,int minNumberExplorations)
		{
			return _saysSimulator.GetMaxExplorationFor(turnTeam, minNumberExplorations);
		}

		private IFigure Resolve(IEnumerable<IFigure> candidates, int topPoints,ISaysStatus saysStatus)
		{
			
			//TODO:
			//if the team mate passed try to close or all the figures have been said
			//check available figures and chose the one with the lowest value
			//if there are not figures then try to close if the player cards value in the siut is higher than the mate ones
			return candidates.First();
		}

		private IEnumerable<IFigure> GetCandidateFigures(ISaysStatus saysStatus, int topPoints)
		{
			return _figures.Where(x => x.IsAvailable(saysStatus,topPoints)).ToList();
		}
	}
}