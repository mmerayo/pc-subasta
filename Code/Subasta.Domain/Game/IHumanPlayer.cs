using Subasta.Domain.Deck;

namespace Subasta.Domain.Game
{

	public delegate ICard MoveSelectionNeeded(IHumanPlayer source,ICard[] validMoves);
	public delegate Declaration? DeclarationSelectionNeeded(IHumanPlayer source, Declaration[] availableDeclarations); //TODO: same for declarations

	public interface IHumanPlayer : IPlayer
	{
		event MoveSelectionNeeded SelectMove;
		event DeclarationSelectionNeeded SelectDeclaration;
	}
}