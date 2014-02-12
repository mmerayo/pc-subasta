using System;
using Subasta.DomainServices.Game;

namespace Subasta.Infrastructure.ApplicationServices
{
	class GameGenerator : IGameGenerator
	{
		private readonly IDeckSuffler _suffler;
		private readonly IGameExplorer _gameExplorer;

		public GameGenerator(IDeckSuffler suffler,IGameExplorer gameExplorer)
		{
			_suffler = suffler;
			_gameExplorer = gameExplorer;
		}

		public Guid GenerateNewGame()
		{
			throw new System.NotImplementedException();
		}
	}
}