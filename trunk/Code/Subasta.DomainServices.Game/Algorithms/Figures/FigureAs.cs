using System;
using System.Linq;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures
{
	internal class FigureAs : IFigure
	{
		public SayKind[] Say
		{
			get { return new[]{SayKind.Una,SayKind.UnaMas}; }
		}

		public int PointsBet
		{
			get { return 1; }
		}

		private ISayCard _potentiallyMarkedCard;

		public bool IsAvailable(ISaysStatus saysStatus)
		{
			ISayCard[] playerCards = saysStatus.GetPlayerCards(saysStatus.Turn);
			bool result = false;

			ISayCard card = playerCards.FirstOrDefault(x => x.Number == 1 && !x.Marked && !x.MarkCandidate);
			if (card != null)
			{
				result = true;
				_potentiallyMarkedCard = card;
				_potentiallyMarkedCard.MarkCandidate = true;

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