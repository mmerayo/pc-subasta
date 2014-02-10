using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game
{
    public interface IValidCardsRule
	{
		ICard[] GetValidMoves(ICard[] playerCards, IHand currentHand);
	}
}