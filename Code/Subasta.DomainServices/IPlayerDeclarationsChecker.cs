using Subasta.Domain;
using Subasta.Domain.Deck;

namespace Subasta.DomainServices
{
    public interface IPlayerDeclarationsChecker
	{
		bool HasDeclarable(Declaration declarable, ISuit trump, ICard[] playerCards);
	}
}