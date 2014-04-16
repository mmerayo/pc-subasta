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

		protected override bool HasCandidates(ISayCard[] playerCards, out ISayCard[] cards)
		{
			foreach (var suit in Suit.Suits)
			{
				var items = playerCards.Where(x => x.Suit.Equals(suit) && !x.Marked && !x.MarkedAsCandidate).ToList();
				if(TryGetCardsWhenMatch(items, new[] {3, 12, 11, 10}, suit,out cards))
					return true;
				if (TryGetCardsWhenMatch(items, new[] { 1, 3, 12, 11 }, suit, out cards))
					return true;
				if (TryGetCardsWhenMatch(items, new[] { 12, 11, 10, 7 }, suit, out cards))
					return true;
			}
			cards = new ISayCard[0];
			return false;
		}
	}
}