using System;
using System.Linq;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Players
{
	internal abstract class AIPlayer : Player
	{
		protected ISimulator Simulator { get; private set; }
		public ISaysSimulator SaysSimulator { get; private set; }
		protected IFiguresSolver FiguresSolver { get; private set; }

		protected AIPlayer(ISimulator simulator,ISaysSimulator saysSimulator,IFiguresSolver figuresSolver)
		{
			Simulator = simulator;
			SaysSimulator = saysSimulator;
			FiguresSolver = figuresSolver;
		}

		public override NodeResult ChooseMove(IExplorationStatus currentStatus, out bool peta)
		{
			NodeResult result = Simulator.GetBest(currentStatus);

			ICard card = result.Status.CurrentHand.CardsByPlaySequence().LastOrDefault(x => x != null);
			peta = card!=null && ResolvePete(card,currentStatus);

			return result;
		}

		private bool ResolvePete(ICard currentCard, IExplorationStatus currentStatus)
		{
			//get highest
			var highestPlayerCard = currentStatus.PlayerCards(PlayerNumber).Where(x => x.Suit.Equals(currentCard.Suit))
				.OrderByDescending(x => x.Value).ThenByDescending(x => x.Number).FirstOrDefault();
			if (highestPlayerCard == null)
				{
				if (!currentCard.Suit.Equals(currentStatus.Trump))
					//marca que falla la siguiente
					return true;
				
				}
			else
			{
				var existing = currentStatus.GetCardsNotYetPlayed()
					.Where(x => x.Suit.Equals(currentCard.Suit) && x != currentCard)
					.OrderByDescending(x => x.Value)
					.ThenByDescending(x => x.Number)
					.FirstOrDefault();
				//marca que tiene la mas alta del palo que tira
				if (existing==null||!highestPlayerCard.IsAbsSmallerThan(existing))
					return true;
			}
			return false;
		}

		public override Domain.Declaration? ChooseDeclaration(IExplorationStatus previousStatus)
		{
			throw new InvalidOperationException("Not valid YET for AI players. Need to correct the interaction");
		}

		public override IFigure ChooseSay(ISaysStatus saysStatus)
		{
			return FiguresSolver.GetFigure(saysStatus);
		}

		public override ISuit ChooseTrump(ISaysStatus saysStatus)
		{
			return SaysSimulator.ChooseTrump(TeamNumber);
		}
	}
}