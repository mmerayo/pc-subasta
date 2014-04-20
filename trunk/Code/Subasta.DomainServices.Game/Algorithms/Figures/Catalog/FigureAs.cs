using System.Collections.Generic;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures.Catalog
{
	internal class FigureAs : Figure
	{
		protected override bool HasAlternativeSay
		{
			get { return true; }
		}
		
		protected override  SayKind SecondarySay
		{
			get { return SayKind.UnaMas; }
		}

		protected override int SecondaryPointsBet
		{
			get { return 1; }
		}

		protected override SayKind PrimarySay
		{
			get { return SayKind.Una; }
		}

		protected override int PrimaryPointsBet
		{
			get { return 1; }
		}

		protected override bool CombinationPerSuit
		{
			get { return true; }
		}

		protected override IEnumerable<int[]> HavingCardNumberCombinations
		{
			get
			{
				yield return new[] {1};
			}
		}

		protected override int[] NotHavingCardNumbers
		{
			get { return new int[0]; }
		}
	}
}