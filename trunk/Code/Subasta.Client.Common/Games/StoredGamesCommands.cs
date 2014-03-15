namespace Subasta.Client.Common.Games
{
	class StoredGamesCommands : IStoredGamesCommands
	{
		private readonly IGameSimulator _currentGameSimulation;
		private readonly IStoredGameReader _gameReader;

		public StoredGamesCommands(IGameSimulator currentGameSimulation,IStoredGameReader gameReader)
		{
			_currentGameSimulation = currentGameSimulation;
			_gameReader = gameReader;
		}

		public void RestoreSimulation(string fileName)
		{
			StoredGameData storedGame=_gameReader.Load(fileName);
			_currentGameSimulation.Load(storedGame);
		
			
		}
	}
}