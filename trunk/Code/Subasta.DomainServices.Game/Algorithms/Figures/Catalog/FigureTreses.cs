using System;
using System.Collections.Generic;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures.Catalog
{
	internal class FigureTreses : Figure
	{
		protected override bool HasAlternativeSay
		{
			get { return false; }
		}
		protected override SayKind PrimarySay
		{
			get { return SayKind.Siete; }
		}

		protected override SayKind AlternativeSay
		{
			get
			{
				throw new InvalidOperationException();
			}
		}

		protected override int PrimaryPointsBet
		{
			get { return 7; }
		}

		protected override int AlternativePointsBet
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
				yield return new[] { 3 };
			}
		}

		protected override int[] NotHavingCardNumbers
		{
			get
			{
				throw new InvalidOperationException();
			}
		}

		protected override int CardRepetitionMin
		{
			get { return 2; }
		}
	}
}