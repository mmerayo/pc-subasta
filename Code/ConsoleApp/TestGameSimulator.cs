using System;
using StructureMap;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.DomainServices.Game;
using Subasta.Infrastructure.Domain;

namespace ConsoleApp
{
	public delegate void StatusChangedHandler(IExplorationStatus status);
	public delegate string InputRequestedHandler();
	public class TestGameSimulator:IGameSimulator
	{
		private readonly IGameExplorer _explorer;
		private readonly IPlayer[] _players=new IPlayer[4];
		private IExplorationStatus _status;

		public event StatusChangedHandler GameStatusChanged;
		public event InputRequestedHandler InputRequested;


		public TestGameSimulator(IGameExplorer explorer)
		{
			_explorer = explorer;
			_players[0] = ObjectFactory.GetInstance<IPlayer>();
			_players[1] = ObjectFactory.GetInstance<IPlayer>(); 
			_players[2] = ObjectFactory.GetInstance<IPlayer>(); 
			_players[3] = ObjectFactory.GetInstance<IPlayer>(); 

		}

		public void Start()
		{
			SetCards();


			int firstPlayer = 1;
			_status = _explorer.GetInitialStatus(Guid.NewGuid(), firstPlayer, 1, _players[0].Cards, _players[1].Cards,
			                                     _players[2].Cards, _players[3].Cards, Suit.FromId('C'), 70);
			
			while (!_status.GameCompleted)
			{
				NextMove();
				OnStatusChanged();
				
				if (_status.CurrentHand.IsCompleted)
				{
					OnInputRequested();
					_status.AddNewHand();
				}

			}

			OnInputRequested();
		}


		private void SetCards()
		{
			for (var i = 0; i < 4; i++)
				_players[i].Cards=LoadCards(i);
		}

		private ICard[] LoadCards(int playerIdx)
		{

			var result=new ICard[10];
			switch (playerIdx)
			{
				case 0:
					result[0] = new Card("B12");
					result[1] = new Card("C12");
					result[2] = new Card("E5");
					result[3] = new Card("C10");
					result[4] = new Card("C2");
					result[5] = new Card("C3");
					result[6] = new Card("C6");
					result[7] = new Card("O11");
					result[8] = new Card("C11");
					result[9] = new Card("C4");
					break;
				case 1:
					result[0] = new Card("B10");
					result[1] = new Card("O3");
					result[2] = new Card("O5");
					result[3] = new Card("O4");
					result[4] = new Card("O2");
					result[5] = new Card("O6");
					result[6] = new Card("O1");
					result[7] = new Card("B6");
					result[8] = new Card("E1");
					result[9] = new Card("B11");
					break;

				case 2:
					result[0] = new Card("B3");
					result[1] = new Card("B5");
					result[2] = new Card("E6");
					result[3] = new Card("E4");
					result[4] = new Card("E2");
					result[5] = new Card("E3");
					result[6] = new Card("O12");
					result[7] = new Card("B4");
					result[8] = new Card("E7");
					result[9] = new Card("C5");
					break;

				case 3:
					result[0] = new Card("O10");
					result[1] = new Card("O7");
					result[2] = new Card("E11");
					result[3] = new Card("E10");
					result[4] = new Card("B7");
					result[5] = new Card("B1");
					result[6] = new Card("C7");
					result[7] = new Card("E12");
					result[8] = new Card("B2");
					result[9] = new Card("C1");
					break;
			}

			return result;
		}

		public bool IsFinished { get; set; }
		public void NextMove()
		{
			var nodeResult = _explorer.Execute(_status, _status.Turn);//TODO: TURN NEEDED??

			int playerPlays = _status.Turn;
			ICard cardAtMove = nodeResult.CardAtMove(playerPlays, _status.Hands.Count);
			_status.CurrentHand.Add(playerPlays,cardAtMove);
		}

		private void OnStatusChanged()
		{
			if (GameStatusChanged != null)
				GameStatusChanged(_status);
		}

		private void OnInputRequested()
		{
			if (InputRequested != null)
				InputRequested();
		}
		
	}
}