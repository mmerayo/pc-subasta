using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Strategies
{
	internal class NonFilteredCandidatesSelector: INonFilteredCandidatesSelector
	{
		private readonly IValidCardsRule _validMoveRule;

		public NonFilteredCandidatesSelector(IValidCardsRule validMoveRule)
		{
			_validMoveRule = validMoveRule;
		}

		public virtual ICard[] GetCandidates(IExplorationStatus currentStatus, byte playerPosition)
		{
			return _validMoveRule.GetValidMoves(currentStatus.PlayerCards(playerPosition), currentStatus.CurrentHand);
		}
	}
}