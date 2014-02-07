namespace Subasta.DomainServices
{
	internal interface IValidCardsRule
	{
		ICard[] GetValidMoves(ICard[] playerCards, IHand currentHand);
	}
}