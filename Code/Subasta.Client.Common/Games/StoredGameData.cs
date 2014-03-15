using Subasta.Domain.Deck;

namespace Subasta.Client.Common.Games
{
	public class StoredGameData
	{
		public ICard[] Player1Cards { get; set; }
		public ICard[] Player2Cards { get; set; }
		public ICard[] Player3Cards { get; set; }
		public ICard[] Player4Cards { get; set; }
	}
}