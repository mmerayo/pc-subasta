using Subasta.Domain.Deck;

namespace ConsoleApp
{
	public interface IGameSimulator
	{
		bool IsFinished { get; set; }
		void NextMove();
		event StatusChangedHandler GameStatusChanged;
		event InputRequestedHandler InputRequested;
		void Start(ICard[] cardsPlayer1, ICard[] cardsPlayer2, ICard[] cardsPlayer3, ICard[] cardsPlayer4);
	}
}