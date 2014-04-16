using System.Linq;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

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

		protected override bool HasCandidates(ISayCard[] playerCards, out ISayCard[] cards)
		{
			ISayCard card = playerCards.FirstOrDefault(x => x.Number == 1 && !x.Marked && !x.MarkCandidate);
			cards = card!=null? new[]{card}:new ISayCard[0];
			return cards.Length > 0;
		}
	}
}