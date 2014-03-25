using Subasta.Domain.Deck;

namespace Subasta.Domain.Game
{

	public delegate ICard MoveSelectionNeeded(IHumanPlayer source); //TODO: same for declarations
	public interface IHumanPlayer : IPlayer
	{
		event MoveSelectionNeeded SelectMove;
	}
}