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
			//TODO: CONFIGURABLE
	
			int minVisits = (saysStatus.Says.Count(x => x.PlayerNum == saysStatus.Turn) + 1)*3000;
			int maxCurrentExploration = _saysSimulator.GetMaxExplorationFor(saysStatus.TurnTeam, minVisits, 0.4);

			IEnumerable<IFigure> candidates = GetCandidateFigures(saysStatus, maxCurrentExploration);
			_saysSimulator.UpdateExplorationListeners();
			return Resolve(candidates, maxCurrentExploration, saysStatus);
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

			//only paso and can bet
			if (candidateFigures.Count == 1 && candidateFigures[0].Say == SayKind.Paso &&
			    saysStatus.PointsBet + 1 <= normalizedTopPoints)
			{
				//if the other team wont reach the normalized top points block them by taking their bet otherwise use current max
				int maxOtherTeam = _saysSimulator.GetMaxExplorationFor(saysStatus.OtherTeam, 0, 0.2)/10;
				int points;
				if (maxOtherTeam < normalizedTopPoints)
				{
					int maxConservative = _saysSimulator.GetMaxExplorationFor(saysStatus.TurnTeam, 0, 0.5) / 10;

					if (saysStatus.PointsBet + 1 <= maxOtherTeam)
					//chooses max between other and a more conservative max as It does not need to be pushed because the opponent cant reach
						{
						points = new[] { maxOtherTeam, maxConservative }.Max();
						}
					else
					{
						points = maxConservative;
					}

				}
				else 
					points = normalizedTopPoints;

				if(saysStatus.PointsBet<points)
				{
					IFigure figure = _figuresCatalog.GetFigureJustPoints(points);
					candidateFigures.Add(figure);
				}
			}

			return candidateFigures;
		}
	}
}