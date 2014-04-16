using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures
{
	internal class FigureCuatro : FigurePaso
	{
		public override SayKind Say
		{
			get { return SayKind.Cuatro; }
		}

		public override SayKind AlternativeSay
		{
			get { return SayKind.Cuatro; }
		}
	}
}