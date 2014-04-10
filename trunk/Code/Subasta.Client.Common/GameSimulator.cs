﻿using System;
using System.Diagnostics;
using Subasta.Domain;
using Subasta.Domain.DalModels;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.DomainServices.Factories;

namespace Subasta.Client.Common
{
	public class GameSimulator : IGameSimulator
	{
		private readonly IGame _game;
		private readonly IDeck _deck;
		private readonly IPlayerFactory _playerFactory;
		private readonly IPlayer[] _players = new IPlayer[4];
		private IExplorationStatus _status;
		private int _firstPlayer = 1;


		public event StatusChangedHandler GameStatusChanged;
		public event StatusChangedHandler GameStarted;
		public event StatusChangedHandler GameCompleted;
		public event MoveSelectionNeeded HumanPlayerMoveSelectionNeeded;
		public event DeclarationSelectionNeeded HumanPlayerDeclarationSelectionNeeded;
		public event InputRequestedHandler InputRequested;
		private Stopwatch _perMoveWatcher;

		public GameSimulator(IGame game, IDeck deck,IPlayerFactory playerFactory)
		{
			_game = game;
			_deck = deck;
			_playerFactory = playerFactory;
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
			_players[0]= _playerFactory.CreatePlayer(1, storedGame);
			_players[1] = _playerFactory.CreatePlayer(2, storedGame);
			_players[2] = _playerFactory.CreatePlayer(3, storedGame);
			_players[3] = _playerFactory.CreatePlayer(4, storedGame);

			foreach (var player in _players)
			{
				if (player.PlayerType == PlayerType.Human)
				{
					var humanPlayer = (IHumanPlayer)player;
					humanPlayer.SelectMove += GameSimulator_SelectMove;
					humanPlayer.SelectDeclaration += humanPlayer_SelectDeclaration;
				}
			}

			FirstPlayer = storedGame.FirstPlayer;
			Trump = storedGame.Trump;
			PointsBet = storedGame.PointsBet;
			TeamBets = storedGame.TeamBets;
		}

		Declaration? humanPlayer_SelectDeclaration(IHumanPlayer source, Declaration[] availableDeclarations)
		{
			return OnDeclarationSelectionNeeded(source, availableDeclarations);
		}

		

		ICard GameSimulator_SelectMove(IHumanPlayer source,ICard[]validMoves)
		{
			return OnMoveSelectionNeeded(source,validMoves);
		}

		private Declaration? OnDeclarationSelectionNeeded(IHumanPlayer source, Declaration[] availableDeclarations)
		{
			if (HumanPlayerDeclarationSelectionNeeded != null)
				return HumanPlayerDeclarationSelectionNeeded(source, availableDeclarations);
			throw new InvalidOperationException("Subscription is needed");
		}

		private ICard OnMoveSelectionNeeded(IHumanPlayer source, ICard[] validMoves)
		{
			if (HumanPlayerMoveSelectionNeeded != null)
				return HumanPlayerMoveSelectionNeeded(source, validMoves);
			throw new InvalidOperationException("Subscription is needed");
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