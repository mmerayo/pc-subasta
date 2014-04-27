using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Subasta.Domain.DalModels;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.DomainServices.Dal;
using Subasta.DomainServices.Game;

namespace Subasta.Client.Common.Game
{
	internal class GameSetHandler : IGameSetHandler
	{
		//TODO: configurable
		private const int GameSetTargetPoints = 100;
		private readonly IGameHandler _gameHandler;
		private IDeck _deck;
		private readonly IDeckSuffler _suffler;
		private readonly IStoredGameWritter _storedGameWritter;

		private readonly int[] _currentPoints = new int[2] {0, 0};
		
		public GameSetHandler(IGameHandler gameHandler,IDeck deck,IDeckSuffler suffler,IStoredGameWritter storedGameWritter)
		{
			_gameHandler = gameHandler;
			_deck = deck;
			_suffler = suffler;
			_storedGameWritter = storedGameWritter;
			SubscribeToGameEvents();
			PlayerDealerNumber = new Random((int) DateTime.UtcNow.Ticks).Next(1, 4);
			Reset();
		}

		public event StatusChangedHandler GameStarted;
		public event StatusChangedHandler GameCompleted;
		public event SaysStatusChangedHandler GameSaysStarted;
		public event SaysStatusChangedHandler GameSaysCompleted;
		public event GameSetStartedHandler GameSetStarted;
		public event GameSetCompletedHandler GameSetCompleted;

		public int GamePoints(int teamNumber)
		{
			return _currentPoints[teamNumber - 1];
		}

		public int PlayerDealerNumber { get; private set; }
		public int FirstPlayer { get { return NextPlayer(PlayerDealerNumber); }}

		public IGameHandler GameHandler
		{
			get { return _gameHandler; }
		}

		private int NextPlayer(int playerNumber)
		{
			if(++playerNumber>4)
				playerNumber = 1;
			return playerNumber;
		}

		readonly ManualResetEvent _startWaitHandle = new ManualResetEvent(true);
		private Task _startTask;
		CancellationTokenSource tokenSource = new CancellationTokenSource();

		public void Start()
		{
			try
			{


				_startTask = Task.Factory.StartNew(() =>
				{
					try
					{
						if (!_startWaitHandle.WaitOne())
							throw new Exception();
						Reset();
						OnGameSetStarted();
					}
					finally
					{
						_startWaitHandle.Set();
					}
				}, tokenSource.Token);
			}
			catch (Exception ex)
			{
				tokenSource.Cancel(true);
				throw;
			}

		}

		private void Reset()
		{
			_currentPoints[0] = _currentPoints[1] = 0;
		}

		private void OnGameSetStarted()
		{
			if (GameSetStarted != null)
				GameSetStarted(this);
			ConfigureNewGame();
		}

		


		private void SubscribeToGameEvents()
		{
			GameHandler.GameStarted += GameHandler_GameStarted;
			GameHandler.GameCompleted += GameHandler_GameCompleted;

			GameHandler.GameSaysCompleted += GameHandler_GameSaysCompleted;
			GameHandler.GameSaysStarted += GameHandler_GameSaysStarted;

		}

		private void GameHandler_GameSaysStarted(ISaysStatus status)
		{
			OnGameSaysStarted(status);
		}

		private void GameHandler_GameSaysCompleted(ISaysStatus status)
		{
			OnGameSaysCompleted(status);
		}

		private void GameHandler_GameCompleted(IExplorationStatus status)
		{
			UpdatePoints(status);
			OnGameCompleted(status);

			if (_currentPoints.Any(x => x >= GameSetTargetPoints))
			{
				OnGameSetCompleted();
			}
			else
			{
				ConfigureNewGame();
			}

		}

		private void UpdatePoints(IExplorationStatus status)
		{
			_currentPoints[status.TeamWinner - 1] += status.NormalizedPointsBet;
		}

		private void ConfigureNewGame()
		{
			PlayerDealerNumber = NextPlayer(PlayerDealerNumber);
			_deck = _suffler.Suffle(_deck);

			var currentPlayer = FirstPlayer;
			var currentIdx = 0;
			var cards = new ICard[4][];

			for (int i = 0; i < 4; i++)
			{
				cards[currentPlayer - 1] = _deck.Cards.Cards.GetRange(currentIdx, 10).ToArray();
				currentIdx += 10;
				currentPlayer = NextPlayer(currentPlayer);
			}

			var newGame = new NewGameConfiguration
			              {
			              	FirstPlayer = FirstPlayer,
			              	Player1Type = PlayerType.Mcts,
			              	Player2Type = PlayerType.Mcts,
			              	Player3Type = PlayerType.Mcts,
			              	Player4Type = PlayerType.Mcts,
			              	Player1Cards = cards[0],
			              	Player2Cards = cards[1],
			              	Player3Cards = cards[2],
			              	Player4Cards = cards[3],
			              };

			GameHandler.ConfigureNewGame(newGame);
			GameHandler.Start();
		}

		private void GameHandler_GameStarted(IExplorationStatus status)
		{
			OnGameStarted(status);
		}

		private void OnGameStarted(IExplorationStatus status)
		{
			_storedGameWritter.Write(GetStoredGameObject(status));

			if (GameStarted != null)
				GameStarted(status);
		}

		private StoredGameData GetStoredGameObject(IExplorationStatus status)
		{
			return new StoredGameData
			             {
			             	FirstPlayer = status.Turn,
			             	Player1Cards = status.PlayerCards(1),
			             	Player1Type = _gameHandler.GetPlayer(1).PlayerType,
			             	Player2Cards = status.PlayerCards(2),
			             	Player2Type = _gameHandler.GetPlayer(2).PlayerType,
			             	Player3Cards = status.PlayerCards(3),
			             	Player3Type = _gameHandler.GetPlayer(3).PlayerType,
			             	Player4Cards = status.PlayerCards(4),
			             	Player4Type = _gameHandler.GetPlayer(4).PlayerType
			             };

		}

		private void OnGameSetCompleted()
		{
			if (GameSetCompleted != null)
				GameSetCompleted(this);
		}

		private void OnGameSaysCompleted(ISaysStatus status)
		{
			if (GameSaysCompleted != null)
				GameSaysCompleted(status);
		}

		private void OnGameSaysStarted(ISaysStatus status)
		{
			if (GameSaysStarted != null)
				GameSaysStarted(status);
		}

		private void OnGameCompleted(IExplorationStatus status)
		{
			if (GameCompleted != null)
				GameCompleted(status);
		}



	}
}