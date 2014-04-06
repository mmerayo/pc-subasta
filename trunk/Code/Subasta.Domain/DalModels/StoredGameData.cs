using Subasta.Domain.Deck;

namespace Subasta.Domain.DalModels
{
	public enum PlayerType
	{
		Human=0,
		Mcts
	}
	public class StoredGameData
	{
		public ICard[] Player1Cards { get; set; }
		public PlayerType Player1Type { get; set; }

		public ICard[] Player2Cards { get; set; }
		public PlayerType Player2Type { get; set; }

		public ICard[] Player3Cards { get; set; }
		public PlayerType Player3Type { get; set; }
		
		public ICard[] Player4Cards { get; set; }
		public PlayerType Player4Type { get; set; }

		public int ExplorationDepth{ get; set; }

		public int FirstPlayer { get; set; }

		public ISuit Trump { get; set; }

		public int TeamBets { get; set; }

		public int PointsBet { get; set; }
	}
}