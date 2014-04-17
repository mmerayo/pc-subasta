using System;
using System.Collections.Generic;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures.Catalog
{
	internal class FigureTreses : Figure
	{

		protected override bool CanBeRepeated
		{
			get { return false; }
		}
		public override SayKind Say
		{
			get { return SayKind.Siete; }
		}

		public override SayKind AlternativeSay
		{
			get
			{
				throw new InvalidOperationException();
			}
		}

		public override int PointsBet
		{
			get { return 7; }
		}

		public override int AlternativePointsBet
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