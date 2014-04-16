using System;
using System.Linq;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

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

		public override int PointsBet
		{
			get { return 2; }
		}

		protected override bool HasCandidates(ISayCard[] playerCards, out ISayCard[] cards)
		{
		throw new NotImplementedException();
		}
	}
}