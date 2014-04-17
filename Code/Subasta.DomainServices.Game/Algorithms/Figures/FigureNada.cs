using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures
{
	internal class FigureNada : FigurePaso
	{
		public override SayKind Say
		{
			get { return SayKind.Cuatro; }
		}

		
	}
}