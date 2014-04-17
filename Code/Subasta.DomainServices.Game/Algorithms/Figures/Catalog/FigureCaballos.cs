using System;
using System.Collections.Generic;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures.Catalog
{
	internal class FigureCaballos : Figure
	{
		public override bool CanBeRepeated
		{
			get { return false; }
		}
		public override SayKind Say
		{
			get { return SayKind.Seis; }
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
			get { return 6; }
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
				yield return new[] {11};
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