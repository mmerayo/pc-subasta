using System;
using System.Collections.Generic;
using System.Linq;
using Subasta.Domain.Game;
using Subasta.DomainServices.Game.Algorithms.Figures.Catalog;

namespace Subasta.DomainServices.Game.Algorithms.Figures
{
	internal class FiguresSolver : IFiguresSolver
	{
		private readonly IFiguresCatalog _figuresCatalog;
		private readonly ISaysSimulator _saysSimulator;

		public FiguresSolver(ISaysSimulator saysSimulator, IFiguresCatalog figuresCatalog)
		{
			_saysSimulator = saysSimulator;
			_figuresCatalog = figuresCatalog;
		}

		public IFigure GetFigure(ISaysStatus saysStatus)
		{
			int minVisits = (saysStatus.Says.Count(x => x.PlayerNum == saysStatus.Turn) + 1)*3000;
			int maxCurrentExploration = GetMaxCurrentExploration(saysStatus.TurnTeam, minVisits);

			IEnumerable<IFigure> candidates = GetCandidateFigures(saysStatus, maxCurrentExploration);
			_saysSimulator.UpdateExplorationListeners();
			return Resolve(candidates, maxCurrentExploration, saysStatus);
		}

		private int GetMaxCurrentExploration(int turnTeam, int minNumberExplorations)
		{
			return _saysSimulator.GetMaxExplorationFor(turnTeam, minNumberExplorations);
		}

		private IFigure Resolve(IEnumerable<IFigure> candidates, int topPoints, ISaysStatus saysStatus)
		{
			//TODO:
			//if the team mate passed try to close or all the figures have been said
			//check available figures and chose the one with the lowest value
			//if there are not figures then try to close if the player cards value in the siut is higher than the mate ones
			//***SE NECESITA LOGICA PARA EN CASO DE QUE HAYA POR EJEMPLO ASES Y PAREJA Y AS O 90 DECIDIR ANTES DE USAR ESTRATEGIA DE MARQUE,
			//FILTRAR()

			IFigure result;


			if (!MatePassed(saysStatus)
				|| (candidates.Count()==1 && candidates.First().Say==SayKind.Paso)
			)
			{
				//Mate still marking,
				result = GiveInformation(candidates);
				MarkSelected(candidates, saysStatus, result);
			}
			else
			{
				//mate passed
				result=new FigureJustPoints(topPoints);
			}
			

			return result;
		}

		private static bool MatePassed(ISaysStatus saysStatus)
		{
			return saysStatus.Says.Count(x => x.Figure.Say == SayKind.Paso && x.PlayerTeamNum == saysStatus.TurnTeam) != 0;
		}

		private static IFigure GiveInformation(IEnumerable<IFigure> candidates)
		{
			IFigure result;
			if (candidates.Any(x => x.PointsBet > 0))
				result = candidates.Where(x => x.PointsBet > 0).OrderBy(x => x.PointsBet).First();
			else
				result = candidates.First();
			return result;
		}

		private static void MarkSelected(IEnumerable<IFigure> candidates, ISaysStatus saysStatus, IFigure result)
		{
			IEnumerable<IFigure> toUnmark = candidates.Where(x => x != result);
			foreach (IFigure figure in toUnmark)
			{
				figure.UnMarkPotentialCandidates();
			}
			result.MarkFigures(saysStatus);
		}

		private IEnumerable<IFigure> GetCandidateFigures(ISaysStatus saysStatus, int topPoints)
		{
			var normalizedTopPoints = (int) Math.Truncate((double) (topPoints/10));
			_figuresCatalog.Init();
			IEnumerable<IFigure> figures = _figuresCatalog.Figures;
			var candidateFigures = figures.Where(x => x.IsAvailable(saysStatus, normalizedTopPoints)).ToList();
			
			return candidateFigures;
		}
	}
}