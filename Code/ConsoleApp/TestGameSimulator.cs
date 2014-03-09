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

	public class TestGameSimulator : IGameSimulator
	{
		private readonly IGameExplorer _explorer;
		private readonly IPlayer[] _players = new IPlayer[4];
		private IExplorationStatus _status;

		public event StatusChangedHandler GameStatusChanged;
		public event InputRequestedHandler InputRequested;


		public TestGameSimulator(IGameExplorer explorer)
		{
			_explorer = explorer;
			_explorer.MaxDepth = 4;
			_players[0] = ObjectFactory.GetInstance<IPlayer>();
			_players[1] = ObjectFactory.GetInstance<IPlayer>();
			_players[2] = ObjectFactory.GetInstance<IPlayer>();
			_players[3] = ObjectFactory.GetInstance<IPlayer>();


		}

		public void Start(ICard[] cardsPlayer1, ICard[] cardsPlayer2, ICard[] cardsPlayer3, ICard[] cardsPlayer4)
		{

			int firstPlayer = 1;
			_players[0].Cards = cardsPlayer1;
			_players[1].Cards = cardsPlayer2;
			_players[2].Cards = cardsPlayer3;
			_players[3].Cards = cardsPlayer4;
			_status = _explorer.GetInitialStatus(Guid.NewGuid(), firstPlayer, 2, _players[0].Cards, _players[1].Cards,
			                                     _players[2].Cards, _players[3].Cards, Suit.FromId('C'), 80);

			while (!_status.IsCompleted)
			{
				NextMove();
				OnStatusChanged();

				if (_status.CurrentHand.IsCompleted)
				{
					_explorer.MaxDepth++;
					//OnInputRequested();
					_status.Turn = _status.CurrentHand.PlayerWinner;
					_status.AddNewHand();

				}

			}

			OnInputRequested();
		}



		public bool IsFinished { get; set; }

		public void NextMove()
		{
			var nodeResult = _explorer.Execute(_status); //TODO: TURN NEEDED??

			int playerPlays = _status.Turn;
			ICard cardAtMove = nodeResult.CardAtMove(playerPlays, _status.Hands.Count);
			_status.CurrentHand.Add(playerPlays, cardAtMove);
			_status.RemovePlayerCard(playerPlays, cardAtMove);
			if (++playerPlays > 4) playerPlays = 1;
			_status.Turn = playerPlays;

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