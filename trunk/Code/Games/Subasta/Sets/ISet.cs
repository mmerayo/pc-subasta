using Games.Deck;

namespace Games.Subasta.Sets
{
	public delegate void SetEventHandler(ISet set);
	public interface ISet
	{
		void Run(IDeck deck, IPlayer[] players, int dealerPosition);

		event SetEventHandler OnCompleted;
	}
}