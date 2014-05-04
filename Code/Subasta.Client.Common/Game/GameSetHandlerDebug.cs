using System.IO;
using Subasta.ApplicationServices.IO;
using Subasta.Domain.Deck;
using Subasta.DomainServices.Dal;
using Subasta.DomainServices.Game;

namespace Subasta.Client.Common.Game
{
	internal class GameSetHandlerDebug : GameSetHandler
	{
		private readonly IPathHelper _pathHelper;
		private readonly IStoredGamesCommands _storedGamesCommands;
		private readonly string[] _files;
		private int _currIdx = int.MinValue;


		public GameSetHandlerDebug(IGameHandler gameHandler, 
		                           IDeck deck,
		                           IDeckSuffler suffler,
		                           IStoredGameWritter storedGameWritter,
		                           IPathHelper pathHelper,
		                           IStoredGamesCommands storedGamesCommands)
			: base(gameHandler, deck, suffler, storedGameWritter)
		{
			_pathHelper = pathHelper;
			_storedGamesCommands = storedGamesCommands;
			string debugPath = _pathHelper.GetApplicationFolderPath("DebugGame", true);

			_files = Directory.GetFiles(debugPath, "*.data");

		}
		protected override void ConfigureNewGame()
		{
			if(_files.Length==0)
				base.ConfigureNewGame();
			else
			{
				if (_currIdx == int.MinValue || _currIdx >= _files.Length - 1)
					_currIdx = 0;

				_storedGamesCommands.RestoreGame(_files[_currIdx++]);
				GameHandler.Start();
			}
		}
	}
}