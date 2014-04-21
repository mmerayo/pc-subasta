using Subasta.Client.Common.Infrastructure;
using Subasta.Domain.Game;

namespace Subasta.Client.Common.Game
{
	class GameSetHandler : IGameSetHandler
	{
		private readonly IGameHandler _gameHandler;

		public GameSetHandler(IGameHandler gameHandler)
		{
			
			_gameHandler = gameHandler;
			SubscribeToGameEvents();
		}

		private void SubscribeToGameEvents()
		{
			
		}

		public void Start()
		{
		}


	}
}