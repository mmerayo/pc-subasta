using Subasta.Domain.Deck;

namespace Subasta.DomainServices.Game
{
    public interface ICardComparer
	{
		ICard Best(ICard current, ICard candidate);
	}
}
