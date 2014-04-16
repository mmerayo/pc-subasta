using System;
using System.Collections.Generic;
using System.Linq;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.Infrastructure.Domain;

namespace Subasta.DomainServices.Game.Algorithms.Figures
{
	internal class FigureParejaSegura : Figure
	{

		protected override bool CanBeRepeated
		{
			get { return false; }
		}
		public override SayKind Say
		{
			get { return SayKind.Dos; }
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