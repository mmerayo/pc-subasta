using Subasta.Client.Common.Game;
using Subasta.Domain.DalModels;
using Subasta.DomainServices.Dal;

namespace Subasta.Client.Common.Storage
{
	class StoredGamesCommands : IStoredGamesCommands
	{
		private readonly IGameHandler _currentGameSimulation;
		private readonly IStoredGameReader _gameReader;

		public StoredGamesCommands(IGameHandler currentGameSimulation,IStoredGameReader gameReader)
		{
			_currentGameSimulation = currentGameSimulation;
			_gameReader = gameReader;
		}

		public void RestoreGame(string fileName)
		{
			StoredGameData storedGame=_gameReader.LoadFromFile(fileName);
			_currentGameSimulation.Load(storedGame);
		
			
		}
	}
}