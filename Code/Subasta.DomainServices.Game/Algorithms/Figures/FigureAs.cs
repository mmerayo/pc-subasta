using System.Linq;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures
{
	internal class FigureAs : IFigure
	{
		public SayKind[] Say
		{
			get { return new[]{SayKind.Una,SayKind.UnaMas}; }
		}

		public bool IsAvailable(ISaysStatus saysStatus, int topPoints)
		{
			return !saysStatus.Says.Any(x => x.PlayerNum == saysStatus.Turn && x.Figure.Say == Say);
		}

	}
}