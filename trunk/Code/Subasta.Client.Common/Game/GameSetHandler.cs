using System;
using Subasta.Domain.Game;

namespace Subasta.Client.Common.Game
{
	internal class GameSetHandler : IGameSetHandler
	{
		private readonly IGameHandler _gameHandler;

		public GameSetHandler(IGameHandler gameHandler)
		{
			_gameHandler = gameHandler;
			SubscribeToGameEvents();
		}

		#region IGameSetHandler Members

		public event StatusChangedHandler GameStarted;
		public event StatusChangedHandler GameCompleted;
		public event SaysStatusChangedHandler GameSaysStarted;
		public event SaysStatusChangedHandler GameSaysCompleted;
		public event GameSetStartedHandler GameSetStarted;
		public event GameSetCompletedHandler GameSetCompleted;

		public void Start()
		{
		}

		#endregion

		private void SubscribeToGameEvents()
		{
			_gameHandler.GameStarted += GameHandler_GameStarted;
			_gameHandler.GameCompleted += GameHandler_GameCompleted;

			_gameHandler.GameSaysCompleted += GameHandler_GameSaysCompleted;
			_gameHandler.GameSaysStarted+= GameHandler_GameSaysStarted;

		}

		private void GameHandler_GameSaysStarted(ISaysStatus status)
		{
			OnGameSaysStarted(status);
		}

		private void OnGameSaysStarted(ISaysStatus status)
		{
			if (GameSaysStarted != null)
				GameSaysStarted(status);
		}

		private void GameHandler_GameSaysCompleted(ISaysStatus status)
		{
			OnGameSaysCompleted(status);
		}

		private void OnGameSaysCompleted(ISaysStatus status)
		{
			if (GameSaysCompleted != null)
				GameSaysCompleted(status);
		}

		private void GameHandler_GameCompleted(IExplorationStatus status)
		{
			OnGameCompleted(status);
		}

		private void OnGameCompleted(IExplorationStatus status)
		{
			if (GameCompleted != null)
				GameCompleted(status);
		}

		private void GameHandler_GameStarted(IExplorationStatus status)
		{
			OnGameStarted(status);
		}

		private void OnGameStarted(IExplorationStatus status)
		{
			if (GameStarted != null)
				GameStarted(status);
		}
	}
}