using System;
using System.Collections.Generic;
using System.Linq;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures
{
	internal class FigurePaso :Figure
	{
		protected override bool CanBeRepeated
		{
			get { return false; }
		}

		public override SayKind Say
		{
			get { return SayKind.Paso;}
		}

		public override SayKind AlternativeSay { get { return Say; } }

		public override int PointsBet
		{
			get { return 0; }
		}

		public override int AlternativePointsBet
		{
			get { return 0; }
		}

		protected override IEnumerable<int[]> HavingCardNumberCombinations
		{
			get
			{
				return new int[0][];
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