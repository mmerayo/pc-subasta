using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures.Catalog
{
	internal class FigureNada : FigurePaso
	{
		protected override SayKind PrimarySay
		{
			get { return SayKind.Cuatro; }
		}

		
	}
}