using Subasta.Domain.Deck;

namespace Subasta.DomainServices
{
    public interface ICardComparer
	{
		ICard Best(ICard current, ICard candidate);
	}
}
