using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.DomainServices.Game;

namespace Subasta.Infrastructure.DomainServices.Game
{
	internal class NonFilteredCandidatesSelector: ICandidatesSelector
	{
		private readonly IValidCardsRule _validMoveRule;

		public NonFilteredCandidatesSelector(IValidCardsRule validMoveRule)
		{
			_validMoveRule = validMoveRule;
		}

		public virtual ICard[] GetCandidates(IExplorationStatus currentStatus, int playerPosition)
		{
			return _validMoveRule.GetValidMoves(currentStatus.PlayerCards(playerPosition), currentStatus.CurrentHand);
		}
	}
}