using System;
using System.Collections.Generic;
using System.Linq;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures
{
	internal abstract class Figure: IFigure
	{
		protected abstract bool CanBeRepeated { get; }

		public abstract SayKind Say { get; }
		public abstract SayKind AlternativeSay { get; }
		public abstract int PointsBet { get; }

		private readonly List<ISayCard> _potentiallyMarkedCards=new List<ISayCard>();

		protected void BookCard(ISayCard card)
		{
			if (card == null) return;

			if(_potentiallyMarkedCards.Contains(card)) throw new ArgumentException();

			card.MarkCandidate = true;
			_potentiallyMarkedCards.Add(card);

		}

		//TODO: TO ABSTRACT
		public void MarkFigures(ISaysStatus saysStatus)
		{
			foreach (var potentiallyMarkedCard in _potentiallyMarkedCards)
			{
				potentiallyMarkedCard.Marked = true;
			}
		}

		public void UnMarkPotentialCandidates()
		{
			foreach (var potentiallyMarkedCard in _potentiallyMarkedCards)
			{
				potentiallyMarkedCard.MarkCandidate = false;
			}
		}

		

		public ICard[] MarkedCards
		{
			get { return _potentiallyMarkedCards.Cast<ICard>().ToArray(); }
		}

		public virtual bool IsAvailable(ISaysStatus saysStatus, int normalizedPoints)
		{
			if (!CanBeRepeated && saysStatus.Says.Any(x => x.PlayerNum == saysStatus.Turn && x.Figure.Say == Say))
				return false;

			ISayCard[] playerCards = saysStatus.GetPlayerCards(saysStatus.Turn);
			bool result = false;
			if (saysStatus.PointsBet + 1 <= normalizedPoints)
			{
				ISayCard[] cards;
				
				if (HasCandidates(playerCards,out cards))
				{
					foreach (var sayCard in cards)
					{
						BookCard(sayCard);
					}

					result = true;

				}
			}
			return result;
		}

		protected abstract bool HasCandidates(ISayCard[] playerCards, out ISayCard[] cards);
		

		
	}
}