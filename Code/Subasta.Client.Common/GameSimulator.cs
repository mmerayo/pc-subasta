using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using StructureMap;
using Subasta.Client.Common.Games;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.DomainServices.Game;
using Subasta.Infrastructure.Domain;

namespace Subasta.Client.Common
{
	public class GameSimulator : IGameSimulator
	{
		private readonly IGameExplorer _explorer;
		private readonly IDeck _deck;
		private readonly IPlayer[] _players = new IPlayer[4];
		private IExplorationStatus _status;

		public event StatusChangedHandler GameStatusChanged;
		public event StatusChangedHandler GameStarted;
		public event InputRequestedHandler InputRequested;

		public GameSimulator(IGameExplorer explorer,IDeck deck)
		{
			_explorer = explorer;
			_deck = deck;
			ReloadPlayers();
		}

		private void ReloadPlayers()
		{
			_players[0] = ObjectFactory.GetInstance<IPlayer>();
			_players[1] = ObjectFactory.GetInstance<IPlayer>();
			_players[2] = ObjectFactory.GetInstance<IPlayer>();
			_players[3] = ObjectFactory.GetInstance<IPlayer>();
		}

		public IPlayer Player1
		{
			get { return _players[0]; }
		}

		public IPlayer Player2
		{
			get { return _players[1]; }
		}

		public IPlayer Player3
		{
			get { return _players[2]; }
		}

		public IPlayer Player4
		{
			get { return _players[3]; }
		}

		public void Start(int depth=int.MinValue)
		{
			if(depth>0)
				_explorer.MaxDepth = depth;
			int firstPlayer = 1;

			ValidateConfiguration();

			_status = _explorer.GetInitialStatus(Guid.NewGuid(), firstPlayer, 2, _players[0].Cards, _players[1].Cards,
			                                     _players[2].Cards, _players[3].Cards, Suit.FromId('C'), 80);
			OnStart();
			while (!_status.IsCompleted)
			{
				NextMove();
				OnStatusChanged();

				if (_status.CurrentHand.IsCompleted)
				{
					_explorer.MaxDepth++;
					//OnInputRequested();
					_status.Turn = _status.CurrentHand.PlayerWinner.Value;
					_status.AddNewHand();

				}

			}

			OnInputRequested();
		}

		
		public void Load(StoredGameData storedGame)
		{
			ReloadPlayers();
			Player1.Cards = storedGame.Player1Cards;
			Player2.Cards = storedGame.Player2Cards;
			Player3.Cards = storedGame.Player3Cards;
			Player4.Cards = storedGame.Player4Cards;
			_explorer.MaxDepth = storedGame.ExplorationDepth;
		}

		private void ValidateConfiguration()
		{
			if (_players.Any(player => player.Cards == null))
			{
				throw new InvalidOperationException("Must set player cards");
			}

			var allCards = new List<ICard>();
			foreach (var player in _players)
			{
				allCards.AddRange(player.Cards);
			}

			var expectedCards = _deck.Cards.Cards;

			foreach (var expectedCard in expectedCards)
			{
				if (allCards.Count(x => x.Equals(expectedCard)) != 1)
					throw new InvalidOperationException(string.Format("{0} is either repeated or it does not exist", expectedCard));
			}

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

		private void OnStart()
		{
			if (GameStarted != null)
				GameStarted(_status);
		}


	}
}