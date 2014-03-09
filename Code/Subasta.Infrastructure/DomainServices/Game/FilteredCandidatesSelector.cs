using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.DomainServices.Game;

namespace Subasta.Infrastructure.DomainServices.Game
{
	internal class FilteredCandidatesSelector : ICandidatesSelector
	{
		private readonly IValidCardsRule _validMoveRule;

		public FilteredCandidatesSelector(IValidCardsRule validMoveRule)
		{
			_validMoveRule = validMoveRule;
		}

		public ICard[] GetCandidates(IExplorationStatus currentStatus, int playerPosition)
		{
			
			//TODO: Create filter for non consecutives
			return
				FilterToMaxMin(_validMoveRule.GetValidMoves(currentStatus.PlayerCards(playerPosition), currentStatus.CurrentHand));

		}

		private static ICard[] FilterToMaxMin(ICard[] source)
		{
			var result = source.ToList();
			foreach (var sameSuit in result.GroupBy(x => x.Suit))
			{
				var ordered = sameSuit.OrderByDescending(x => x.Value).ThenByDescending(x => x.Number).ToList();
				RemoveCandidatesInTheMiddle(result, ordered);
			}
			
			return result.ToArray();
		}

		private static void RemoveCandidatesInTheMiddle(List<ICard> result, List<ICard> ordered)
		{
			var current = ordered.Skip(1);
			current = current.Take(current.Count() - 1);
			foreach (var card in current)
			{
				result.Remove(card);
			}
		}
	}
}