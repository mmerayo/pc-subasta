using System.Collections.Generic;
using Subasta.Domain.Deck;

namespace Subasta.Domain.Game
{

	public delegate ICard MoveSelectionNeeded(IHumanPlayer source,ICard[] validMoves, out bool peta);
	public delegate Declaration? DeclarationSelectionNeeded(IHumanPlayer source, Declaration[] availableDeclarations,IExplorationStatus status); 
	public delegate IFigure SayNeededEvent(IHumanPlayer source,ISaysStatus saysStatus);
	public delegate ISuit TrumpNeededEvent(IHumanPlayer source);


	public interface IHumanPlayer : IPlayer
	{
		event MoveSelectionNeeded SelectMove;
		event DeclarationSelectionNeeded SelectDeclaration;
		event SayNeededEvent SelectSay;
		event TrumpNeededEvent ChooseTrumpRequest;
		IEnumerable<Declaration> GetUserDeclarables(IExplorationStatus status);
	}
}