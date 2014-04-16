using System;
using System.Collections.Generic;
using System.Linq;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.Infrastructure.Domain;

namespace Subasta.DomainServices.Game.Algorithms.Figures
{
	internal class FigureCaballos : Figure
	{

		protected override bool CanBeRepeated
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