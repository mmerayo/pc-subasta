using System.Collections.Generic;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures.Catalog
{
	internal class FigureParejaNoSegura : Figure
	{

		protected override bool CanBeRepeated
		{
			get { return false; }
		}
		public override SayKind Say
		{
			get { return SayKind.Cinco; }
		}

		public override SayKind AlternativeSay { get { return SayKind.UnaMas; } }

		public override int AlternativePointsBet
		{
			get { return 1; }
		}

		public override int PointsBet
		{
			get { return 2; }
		}

		protected override IEnumerable<int[]> HavingCardNumberCombinations
		{
			get
			{
				yield return new[] {12, 11};
			}
		}

		protected override int[] NotHavingCardNumbers
		{
			get { return new []{3,10}; }
		}

		protected override bool CombinationPerSuit
		{
			get { return true; }
		}
	}
}