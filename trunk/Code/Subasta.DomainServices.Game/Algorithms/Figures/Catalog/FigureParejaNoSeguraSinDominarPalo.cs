using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures.Catalog
{
	internal class FigureParejaNoSeguraSinDominarPalo : FigureParejaNoSegura
	{
		protected override int PrimaryPointsBet
		{
			get { return 8; }
		}

		protected override SayKind PrimarySay
		{
			get { return SayKind.Ocho; }
		}
	}
}