using System;
using Subasta.Client.Common.Infrastructure;
using Subasta.Domain;
using Subasta.Domain.DalModels;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.DomainServices.Factories;

namespace Subasta.Client.Common.Game
{
	public class GameHandler : IGameHandler
	{
		private readonly IGame _game;
		private readonly IPlayerFactory _playerFactory;
		private readonly IPlayer[] _players = new IPlayer[4];
		private IExplorationStatus _status;
		private int _firstPlayer = 1;


		public event StatusChangedHandler GameStatusChanged;
		public event SaysStatusChangedHandler GameSaysStatusChanged;
		public event SaysStatusChangedHandler GameSaysStarted;
		public event SaysStatusChangedHandler GameSaysCompleted;
		

		public event StatusChangedHandler GameStarted;
		public event StatusChangedHandler GameCompleted;
		public event StatusChangedHandler HandCompleted;
		public event MoveSelectionNeeded HumanPlayerMoveSelectionNeeded;
		public event DeclarationSelectionNeeded HumanPlayerDeclarationSelectionNeeded;
		public event SayNeededEvent HumanPlayerSayNeeded;
		public event TrumpNeededEvent HumanPlayerTrumpNeeded;
		private ISaysStatus _saysStatus;

		public GameHandler(IGame game, IPlayerFactory playerFactory)
		{
			_game = game;
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

		public IPlayer GetPlayer(int playerNum)
		{
			return _players[playerNum - 1];
		}


		public ISuit Trump { get { return _status.Trump; } }
		public int PointsBet { get { return _status.PointsBet; } }


		public int TeamBets { get { return _status.TeamBets; } }

		public void Start()
		{
			_game.SetGameInfo(Player1, Player2, Player3, Player4, FirstPlayer);
			
			SubscribeToEvents();


			_game.StartGame();
		}

		private void SubscribeToEvents()
		{
			_game.GameStatusChanged += _game_GameStatusChanged;
			_game.GameStarted += _game_GameStarted;
			_game.GameCompleted += _game_GameCompleted;
			_game.HandCompleted += _game_HandCompleted;

			_game.GameSaysStatusChanged += _game_GameSaysStatusChanged;
			_game.GameSaysStarted += _game_GameSaysStarted;
			_game.GameSaysCompleted += _game_GameSaysCompleted;
		}

		void _game_HandCompleted(IExplorationStatus status)
		{
			if (HandCompleted != null)
				HandCompleted(status);
		}

		void _game_GameSaysCompleted(ISaysStatus status)
		{
			_saysStatus = status;
			OnSaysCompleted();

		}

		

		void _game_GameSaysStarted(ISaysStatus status)
		{
			_saysStatus = status;
			OnSaysStarted();
		}
		

		void _game_GameSaysStatusChanged(ISaysStatus status)
		{
			_saysStatus = status;
			OnSaysChanged();
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
					humanPlayer.SelectMove += GameHandler_SelectMove;
					humanPlayer.SelectDeclaration += humanPlayer_SelectDeclaration;
					humanPlayer.SelectSay += humanPlayer_SelectSay;
					humanPlayer.ChooseTrumpRequest+=humanPlayer_ChooseTrumpRequest;
				}
			}

			FirstPlayer = storedGame.FirstPlayer;
		}

		public void ConfigureNewGame(NewGameConfiguration gameConfiguration)
		{
			Load(gameConfiguration);
		}


		private ISuit humanPlayer_ChooseTrumpRequest(IHumanPlayer source)
		{
			return OnTrumpSelectionNeeded(source);
		}

		IFigure humanPlayer_SelectSay(IHumanPlayer source,ISaysStatus saysStatus)
		{
			return OnSaySelectionNeeded(source,saysStatus);
		}

		Declaration? humanPlayer_SelectDeclaration(IHumanPlayer source, Declaration[] availableDeclarations)
		{
			return OnDeclarationSelectionNeeded(source, availableDeclarations);
		}

		ICard GameHandler_SelectMove(IHumanPlayer source,ICard[]validMoves, out bool peta)
		{
			return OnMoveSelectionNeeded(source,validMoves, out  peta);
		}

		private Declaration? OnDeclarationSelectionNeeded(IHumanPlayer source, Declaration[] availableDeclarations)
		{
			if (HumanPlayerDeclarationSelectionNeeded != null)
				return HumanPlayerDeclarationSelectionNeeded(source, availableDeclarations);
			throw new InvalidOperationException("Subscription is mandatory");
		}

		private ICard OnMoveSelectionNeeded(IHumanPlayer source, ICard[] validMoves, out bool peta)
		{
			if (HumanPlayerMoveSelectionNeeded != null)
				return HumanPlayerMoveSelectionNeeded(source, validMoves,out peta);
			throw new InvalidOperationException("Subscription is mandatory");
		}

		private IFigure OnSaySelectionNeeded(IHumanPlayer source, ISaysStatus saysStatus)
		{
			if (HumanPlayerSayNeeded != null)
				return HumanPlayerSayNeeded(source,saysStatus);
			throw new InvalidOperationException("Subscription is mandatory");
		}

		private ISuit OnTrumpSelectionNeeded(IHumanPlayer source)
		{
			if (HumanPlayerTrumpNeeded != null)
				return HumanPlayerTrumpNeeded(source);
			throw new InvalidOperationException("Subscription is mandatory");
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
				GameStatusChanged(_status);
		}
		

		private void OnStart()
		{
			if (GameStarted != null)
				GameStarted(_status);
		}

		private void OnCompleted()
		{
			if (GameCompleted != null)
				GameCompleted(_status);
		}

		private void OnSaysChanged()
		{
			if (GameSaysStatusChanged != null)
				GameSaysStatusChanged(_saysStatus);
		}

		private void OnSaysCompleted()
		{
			if (GameSaysCompleted != null)
				GameSaysCompleted(_saysStatus);
		}
		private void OnSaysStarted()
		{
			if (GameSaysStarted != null)
				GameSaysStarted(_saysStatus);
		}
	}
}