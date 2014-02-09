using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices
{
    public interface IValidCardsRule
	{
		ICard[] GetValidMoves(ICard[] playerCards, IHand currentHand);
	}
}