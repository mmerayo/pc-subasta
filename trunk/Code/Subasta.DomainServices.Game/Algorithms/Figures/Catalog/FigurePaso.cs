using System;
using System.Collections.Generic;
using System.Linq;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures.Catalog
{
	internal class FigurePaso :Figure
	{
		protected override bool HasAlternativeSay
		{
			get { return false; }
		}

		protected override SayKind PrimarySay
		{
			get { return SayKind.Paso;}
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
			get { return 0; }
		}

		protected override int SecondaryPointsBet
		{
			get
			{
				throw new InvalidOperationException();
			}
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

		public override bool IsAvailable(ISaysStatus saysStatus, int normalizedTopPoints)
		{
			return !saysStatus.Says.Any(x => x.PlayerNum == saysStatus.Turn && x.Figure.Say == SayKind.Paso);

		}
	}
}