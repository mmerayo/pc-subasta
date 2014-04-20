using System.Collections.Generic;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures.Catalog
{
	internal class FigureParejaNoSegura : Figure
	{
		protected override bool HasAlternativeSay
		{
			get { return true; }
		}
		protected override SayKind PrimarySay
		{
			get { return SayKind.Cinco; }
		}

		protected override SayKind SecondarySay { get { return SayKind.UnaMas; } }

		protected override int SecondaryPointsBet
		{
			get { return 1; }
		}

		protected override int PrimaryPointsBet
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