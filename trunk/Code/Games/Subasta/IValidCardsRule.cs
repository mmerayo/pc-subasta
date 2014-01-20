using Games.Deck;

namespace Games.Subasta
{
	internal interface IValidCardsRule
	{
		ICard[] GetValidMoves(ICard[] playerCards, IHand currentHand);
	}
}