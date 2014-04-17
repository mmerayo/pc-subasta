using System;
using System.Collections.Generic;
using System.Linq;
using StructureMap;
using Subasta.Domain.Game;
using Subasta.DomainServices.Game.Algorithms.Figures.Catalog;

namespace Subasta.DomainServices.Game.Algorithms.Figures
{
	internal class FiguresSolver : IFiguresSolver
	{
		private readonly ISaysSimulator _saysSimulator;
		private readonly IFiguresCatalog _figuresCatalog;

		public FiguresSolver(ISaysSimulator saysSimulator,IFiguresCatalog figuresCatalog )
		{
			_saysSimulator = saysSimulator;
			_figuresCatalog = figuresCatalog;
		}

		public IFigure GetFigure(ISaysStatus saysStatus)
		{
			int minVisits = (saysStatus.Says.Count(x=>x.PlayerNum==saysStatus.Turn)+1)*3000;
			var maxCurrentExploration = GetMaxCurrentExploration(saysStatus.TurnTeam,minVisits);

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
			//***SE NECESITA LOGICA PARA EN CASO DE QUE HAYA POR EJEMPLO ASES Y PAREJA Y AS O 90 DECIDIR ANTES DE USAR ESTRATEGIA DE MARQUE,
				//FILTRAR()

			IFigure result;
			if (candidates.Any(x => x.PointsBet > 0))
				result = candidates.Where(x => x.PointsBet > 0).OrderBy(x=>x.PointsBet).First();
			else
				result = candidates.First();
			var toUnmark = candidates.Where(x => x != result);
			foreach (var figure in toUnmark)
			{
				figure.UnMarkPotentialCandidates();
			}
			result.MarkFigures(saysStatus);
			return result;
		}

		private IEnumerable<IFigure> GetCandidateFigures(ISaysStatus saysStatus, int topPoints)
		{
			int normalizedPoints = (int)Math.Truncate((double)(topPoints/10));
			IEnumerable<IFigure> candidateFigures = _figuresCatalog.Figures.Where(x => x.IsAvailable(saysStatus,normalizedPoints)).ToList();
			
			return candidateFigures;
		}
	}
}