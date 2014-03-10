using Subasta.Domain.Deck;

namespace Subasta.Client.Common
{
	class Player : IPlayer
	{
		public ICard[] Cards { get; set; }
	}
}