using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.Client.Common
{
	public delegate void StatusChangedHandler(IExplorationStatus status);
	public delegate string InputRequestedHandler();
	
	public interface IGameSimulator
	{
		bool IsFinished { get; set; }
		void NextMove();
		event StatusChangedHandler GameStatusChanged;
		event InputRequestedHandler InputRequested;
		void Start(ICard[] cardsPlayer1, ICard[] cardsPlayer2, ICard[] cardsPlayer3, ICard[] cardsPlayer4, int depth);
	}
}