using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.DomainServices.Game;

namespace Subasta.Infrastructure.DomainServices.Game
{
	internal class FilteredCandidatesSelector :NonFilteredCandidatesSelector
	{

		public FilteredCandidatesSelector(IValidCardsRule validMoveRule):base(validMoveRule)
		{
		}

		public override ICard[] GetCandidates(IExplorationStatus currentStatus, int playerPosition)
		{
			//TODO: Create filter for non consecutives
			ICard[] validMoves = base.GetCandidates(currentStatus, playerPosition);
			return
				FilterToMaxMin(validMoves);
		}

		private static ICard[] FilterToMaxMin(ICard[] source)
		{
			var result = source.ToList();
			foreach (var sameSuit in result.GroupBy(x => x.Suit))
			{
				var ordered = sameSuit.OrderBy(x => x.Value).ThenBy(x => x.Number).ToList();
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