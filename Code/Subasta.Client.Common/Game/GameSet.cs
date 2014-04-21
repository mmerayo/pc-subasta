using Subasta.Client.Common.Infrastructure;
using Subasta.Domain.Game;

namespace Subasta.Client.Common.Game
{
	class GameSet : IGameSet
	{
		
		private readonly IGame _game;

		public GameSet(IGame game)
		{
			
			_game = game;

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