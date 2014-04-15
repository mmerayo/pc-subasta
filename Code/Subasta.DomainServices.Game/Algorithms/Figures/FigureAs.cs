using System;
using System.Linq;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures
{
	internal class FigureAs : IFigure
	{
		public SayKind Say
		{
			get { return SayKind.Una; }
		}

		public SayKind AlternativeSay { get { return SayKind.UnaMas; } }

		public int PointsBet
		{
			get { return 1; }
		}

		private ISayCard _potentiallyMarkedCard;

		public bool IsAvailable(ISaysStatus saysStatus, int normalizedPoints)
		{
			ISayCard[] playerCards = saysStatus.GetPlayerCards(saysStatus.Turn);
			bool result = false;
			if (saysStatus.PointsBet + 1 <= normalizedPoints)
			{
				ISayCard card = playerCards.FirstOrDefault(x => x.Number == 1 && !x.Marked && !x.MarkCandidate);
				if (card != null)
				{
					result = true;
					_potentiallyMarkedCard = card;
					_potentiallyMarkedCard.MarkCandidate = true;
				}
			}
			return result;
		}

		public void MarkFigures(ISaysStatus saysStatus)
		{
			_potentiallyMarkedCard.Marked = true;
		}

		public void UnMarkPotentialCandidates()
		{
			_potentiallyMarkedCard.MarkCandidate = false;
		}
	}
}