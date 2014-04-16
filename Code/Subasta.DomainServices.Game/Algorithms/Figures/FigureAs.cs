using System.Collections.Generic;
using System.Linq;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.Infrastructure.Domain;

namespace Subasta.DomainServices.Game.Algorithms.Figures
{
	internal class FigureAs : Figure
	{

		protected override bool CanBeRepeated
		{
			get { return true; }
		}
		public override SayKind Say
		{
			get { return SayKind.Una; }
		}

		public override SayKind AlternativeSay { get { return SayKind.UnaMas; } }

		public override int PointsBet
		{
			get { return 1; }
		}

		public override int AlternativePointsBet
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