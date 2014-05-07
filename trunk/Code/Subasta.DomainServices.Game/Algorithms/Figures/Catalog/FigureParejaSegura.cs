using System.Collections.Generic;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures.Catalog
{
	internal class FigureParejaSegura : Figure
	{
		protected override bool HasAlternativeSay
		{
			get { return true; }
		}
		protected override SayKind PrimarySay
		{
			get { return SayKind.Dos; }
		}

		protected override SayKind SecondarySay { get { return SayKind.UnaMas; } }

		protected override byte SecondaryPointsBet
		{
			get { return 1; }
		}

		protected override byte PrimaryPointsBet
		{
			get { return 2; }
		}

		protected override IEnumerable<int[]> HavingCardNumberCombinations
		{
			get
			{
				yield return new[] {3, 12, 11, 10};
				yield return new[] {1, 3, 12, 11};
				yield return new[] {12, 11, 10, 7};
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