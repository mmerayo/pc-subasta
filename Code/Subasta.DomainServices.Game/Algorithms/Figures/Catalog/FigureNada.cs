using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures.Catalog
{
	internal class FigureNada : FigurePaso
	{
		protected override SayKind PrimarySay
		{
			get { return SayKind.Cuatro; }
		}

		protected override byte PrimaryPointsBet
		{
			get { return 4; }
		}

		public override bool IsAvailable(ISaysStatus saysStatus, byte normalizedTopPoints)
		{
			return saysStatus.PointsBet < 4;
		}
	}
}