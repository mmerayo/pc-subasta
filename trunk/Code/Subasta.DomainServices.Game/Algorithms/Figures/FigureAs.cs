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

		private string _potentiallyMarkedCardCode;

		public bool IsAvailable(ISaysStatus saysStatus, int topPoints)
		{
			ISayCard[] playerCards = saysStatus.GetPlayerCards(saysStatus.Turn);
			bool result = false;
			if(saysStatus.PointsBet+1<topPoints)
			{
				ISayCard card = playerCards.FirstOrDefault(x => x.Number == 1 && !x.Marked);
				if (card != null)
				{
					result = true;
					_potentiallyMarkedCardCode = card.ToShortString();
				}
			}
			return result;
		}

		public void MarkFigures(ISaysStatus saysStatus)
		{
			var card = saysStatus.GetPlayerCards(saysStatus.Turn).Single(x=>x.ToShortString().Equals(_potentiallyMarkedCardCode));
			card.Marked = true;
		}
	}
}