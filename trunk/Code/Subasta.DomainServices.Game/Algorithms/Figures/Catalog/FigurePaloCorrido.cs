using System;
using System.Collections.Generic;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures.Catalog
{
	internal class FigurePaloCorrido : Figure
	{
		protected override bool HasAlternativeSay
		{
			get { return false; }
		}
		protected override SayKind PrimarySay
		{
			get { return SayKind.Nueve; }
		}

		protected override SayKind SecondarySay
		{
			get { throw new InvalidOperationException();}
		}

		protected override int SecondaryPointsBet
		{
			get { throw new InvalidOperationException(); }
		}

		protected override int PrimaryPointsBet
		{
			get { return 9; }
		}

		protected override IEnumerable<int[]> HavingCardNumberCombinations
		{
			get
			{
				yield return new[] {1,3, 12};
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