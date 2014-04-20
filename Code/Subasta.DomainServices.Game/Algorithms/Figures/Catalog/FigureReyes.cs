using System;
using System.Collections.Generic;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures.Catalog
{
	internal class FigureReyes : Figure
	{
		protected override bool HasAlternativeSay
		{
			get { return false; }
		}
		protected override SayKind PrimarySay
		{
			get { return SayKind.Doce; }
		}

		protected override SayKind SecondarySay
		{
			get
			{
				throw new InvalidOperationException();
			}
		}

		protected override int PrimaryPointsBet
		{
			get { return 12; }
		}

		protected override int SecondaryPointsBet
		{
			get
			{
				throw new InvalidOperationException();
			}
		}

		protected override bool CombinationPerSuit
		{
			get { return false; }
		}

		protected override IEnumerable<int[]> HavingCardNumberCombinations
		{
			get
			{
				yield return new[] {12};
			}
		}

		protected override int[] NotHavingCardNumbers
		{
			get
			{
				throw new InvalidOperationException();
			}
		}
	}
}