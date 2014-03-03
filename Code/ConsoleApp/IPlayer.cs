using Subasta.Domain.Deck;

namespace ConsoleApp
{
	public interface IPlayer
	{
		void SetCards(ICard[] toArray);
	}
}