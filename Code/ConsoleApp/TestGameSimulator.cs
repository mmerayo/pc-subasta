using System;
using Subasta.Domain.Deck;
using Subasta.DomainServices.Game;

namespace ConsoleApp
{
	public class TestGameSimulator:IGameSimulator
	{
		private readonly IPlayer[] _players=new IPlayer[4];
		public TestGameSimulator(IPlayer player1,IPlayer player2,IPlayer player3,IPlayer player4)
		{
			_players[0] = player1;
			_players[1] = player2;
			_players[2] = player3;
			_players[3] = player4;
		}

		public void Start()
		{

			for (var i = 0; i < 4;i++ )
				_players[i].SetCards(LoadCards(i));
			
		}

		private ICard[] LoadCards(int playerIdx)
		{

			var result=new ICard[10];
			switch (playerIdx)
			{
				case 0:
					break;
				case 1:
					break;
				case 2:
					break;
				case 3:
					break;
			}

			return result;
		}

		public bool IsFinished { get; set; }
		public void NextMove()
		{
			throw new System.NotImplementedException();
		}
	}
}