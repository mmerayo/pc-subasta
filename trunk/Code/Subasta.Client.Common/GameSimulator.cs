using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using StructureMap;
using Subasta.Client.Common.Games;
using Subasta.Domain;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.DomainServices.Game;
using Subasta.DomainServices.Game.Players;

namespace Subasta.Client.Common
{
	public class GameSimulator : IGameSimulator
	{
		private readonly IGame _game;
		private readonly IDeck _deck;
		private readonly IPlayer[] _players = new IPlayer[4];
		private IExplorationStatus _status;
		private int _firstPlayer = 1;


		public event StatusChangedHandler GameStatusChanged;
		public event StatusChangedHandler GameStarted;
		public event StatusChangedHandler GameCompleted;
		public event InputRequestedHandler InputRequested;
		private Stopwatch _perMoveWatcher;

		public GameSimulator(IGame game, IDeck deck)
		{
			_game = game;
			_deck = deck;
			ReloadPlayers();
		}

		private void ReloadPlayers()
		{
			_players[0] = ObjectFactory.GetInstance<IMctsPlayer>();
			_players[0].Name = "Player 1";
			_players[0].TeamNumber = 1;

			_players[1] = ObjectFactory.GetInstance<IMctsPlayer>();
			_players[1].Name = "Player 2";
			_players[1].TeamNumber = 2;

			_players[2] = ObjectFactory.GetInstance<IMctsPlayer>();
			_players[2].Name = "Player 3";
			_players[2].TeamNumber = 1;

			_players[3] = ObjectFactory.GetInstance<IMctsPlayer>();
			_players[3].Name = "Player 4";
			_players[3].TeamNumber = 2;

		}

		public IPlayer Player1 //I PLAYER TO BE KEPT IN DOMAIN and holds the style
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



		public ISuit Trump { get; private set; }
		public int PointsBet { get; private set; }


		public int TeamBets { get; private set; }

		public void Start(int depth = int.MinValue)
		{
			_game.SetGameInfo(Player1, Player2, Player3, Player4, FirstPlayer, TeamBets, Trump, PointsBet);
			_game.GameStatusChanged +=_game_GameStatusChanged;
			_game.GameStarted += _game_GameStarted;
			_game.GameCompleted += new GameStatusChangedHandler(_game_GameCompleted);
			_game.StartGame();
		}

		void _game_GameCompleted(IExplorationStatus status)
		{
			_status = status;
			OnCompleted();
		}

		void _game_GameStarted(IExplorationStatus status)
		{
			_status = status;
			OnStart();
		}

		void _game_GameStatusChanged(IExplorationStatus status)
		{
			_status = status;
			OnStatusChanged();
		}



		public void Load(StoredGameData storedGame)
		{
			ReloadPlayers();
			Player1.Cards = storedGame.Player1Cards;
			Player2.Cards = storedGame.Player2Cards;
			Player3.Cards = storedGame.Player3Cards;
			Player4.Cards = storedGame.Player4Cards;
			FirstPlayer = storedGame.FirstPlayer;
			Trump = storedGame.Trump;
			PointsBet = storedGame.PointsBet;
			TeamBets = storedGame.TeamBets;
		}

		public bool IsFinished { get; set; }

		public int FirstPlayer
		{
			get { return _firstPlayer; }
			private set { _firstPlayer = value; }
		}



		private void OnStatusChanged()
		{
			if (GameStatusChanged != null)
				GameStatusChanged(_status, TimeSpan.Zero);
		}


		private void OnInputRequested()
		{
			if (InputRequested != null)
				InputRequested();
		}

		private void OnStart()
		{
			if (GameStarted != null)
				GameStarted(_status, TimeSpan.Zero);
		}

		private void OnCompleted()
		{
			if (GameCompleted != null)
				GameCompleted(_status, TimeSpan.Zero);
		}
	}
}