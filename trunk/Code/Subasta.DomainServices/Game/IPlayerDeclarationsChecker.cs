using Subasta.Domain;
using Subasta.Domain.Deck;

namespace Subasta.DomainServices.Game
{
    public interface IPlayerDeclarationsChecker
	{
		bool HasDeclarable(Declaration declarable, ISuit trump, ICard[] playerCards);
	}
}