using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Subasta.ApplicationServices.Extensions;
using Subasta.Client.Common.Media;
using Subasta.Domain.DalModels;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.DomainServices.Dal;
using Subasta.DomainServices.Game;
using log4net;

namespace Subasta.Client.Common.Game
{
	internal class GameSetHandler : IGameSetHandler
	{
		//TODO: configurable
		private const int GameSetTargetPoints = 100;
		private static readonly ILog Logger = LogManager.GetLogger(typeof (GameSetHandler));
		private readonly int[] _currentPoints = new int[2] {0, 0};
		private readonly IGameHandler _gameHandler;
		private readonly List<List<IExplorationStatus>> _sets = new List<List<IExplorationStatus>>();
		private readonly IDeckShuffler _shuffler;
		private readonly ISoundPlayer _soundPlayer;
		private readonly ManualResetEvent _startWaitHandle = new ManualResetEvent(true);
		private readonly IStoredGameWritter _storedGameWritter;
		private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();
		private IDeck _deck;

		public GameSetHandler(IGameHandler gameHandler, IDeck deck,
		                      IDeckShuffler Shuffler, IStoredGameWritter storedGameWritter, ISoundPlayer soundPlayer)
		{
			_gameHandler = gameHandler;
			_deck = deck;
			_shuffler = Shuffler;
			_storedGameWritter = storedGameWritter;
			_soundPlayer = soundPlayer;

			SubscribeToGameEvents();
			PlayerDealerNumber = new Random((int) DateTime.UtcNow.Ticks).Next(1, 4);
			Reset();
		}

		#region IGameSetHandler Members

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

		public int FirstPlayer
		{
			get { return NextPlayer(PlayerDealerNumber); }
		}

		public IGameHandler GameHandler
		{
			get { return _gameHandler; }
		}

		public List<List<IExplorationStatus>> Sets
		{
			get { return _sets; }
		}

		public void Start()
		{
			try
			{
				Task.Factory.StartNew(() =>
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
				                      }, tokenSource.Token).LogTaskException(Logger);
			}
			catch (Exception ex)
			{
				Logger.Error("Start", ex);
				tokenSource.Cancel(true);
				throw;
			}
		}

		#endregion

		private int NextPlayer(int playerNumber)
		{
			if (++playerNumber > 4)
				playerNumber = 1;
			return playerNumber;
		}

		private void Reset()
		{
			_currentPoints[0] = _currentPoints[1] = 0;
		}

		private void OnGameSetStarted()
		{
			//add set to record
			Sets.Add(new List<IExplorationStatus>());

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
			if (Sets.Any())
				Sets.Last().Add(status);
			UpdatePoints(status);
			OnGameCompleted(status);

			//records the status in the current set


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

		protected virtual void ConfigureNewGame()
		{
			PlayerDealerNumber = NextPlayer(PlayerDealerNumber);
			_soundPlayer.Play(GameSoundType.Shuffle);
			_deck = _shuffler.Shuffle(_deck);

			int currentPlayer = FirstPlayer;
			int currentIdx = 0;
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
			              	Player1Type = PlayerType.Human,
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