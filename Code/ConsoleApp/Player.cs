using Subasta.Domain.Deck;

namespace ConsoleApp
{
	class Player : IPlayer
	{
		public ICard[] Cards { get; set; }
	}
}