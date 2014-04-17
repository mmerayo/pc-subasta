using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures
{
	internal class FigureNoSeguraSinDominarPalo : FigureParejaNoSegura
	{
		public override int PointsBet
		{
			get { return 8; }
		}

		public override SayKind Say
		{
			get { return SayKind.Ocho; }
		}
	}
}