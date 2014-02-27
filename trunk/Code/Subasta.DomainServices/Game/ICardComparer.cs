using Subasta.Domain.Deck;

namespace Subasta.DomainServices.Game
{
    public interface ICardComparer
	{
		ICard Best(ISuit trump, ICard current, ICard candidate);
	}
}
