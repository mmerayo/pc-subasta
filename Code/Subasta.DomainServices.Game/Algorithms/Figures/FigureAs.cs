using System;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures
{
	internal class FigureAs : IFigure
	{
		public SayKind[] Say
		{
			get { return new[]{SayKind.Una,SayKind.UnaMas}; }
		}

		public int PointsBet
		{
			get { return 1; }
		}

		public bool IsAvailable(ISaysStatus saysStatus, int topPoints)
		{
			//ensure everything works first
			throw new NotImplementedException();
			//return !saysStatus.Says.Any(x => x.PlayerNum == saysStatus.Turn && x.Figure.Say == Say);
		}

	}
}