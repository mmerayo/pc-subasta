using System.Collections.Generic;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures.Catalog
{
	internal class FigureParejaConAs : Figure
	{
		public override bool CanBeRepeated
		{
			get { return true; }
		}
		public override SayKind Say
		{
			get { return SayKind.Tres; }
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
				yield return new[] {1, 12, 11};
			}
		}

		protected override int[] NotHavingCardNumbers
		{
			get { return new int[0]; }
		}

		protected override bool CombinationPerSuit
		{
			get { return true; }
		}
	}
}