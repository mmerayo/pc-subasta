using System.Collections.Generic;
using Subasta.Domain.Game;

namespace Subasta.Client.Common.Game
{

	public delegate void GameSetStartedHandler(IGameSetHandler sender);
	public delegate void GameSetCompletedHandler(IGameSetHandler sender);
	//PARTIDA
	public interface IGameSetHandler
	{
		event StatusChangedHandler GameStarted;
		event StatusChangedHandler GameCompleted;
		
		event SaysStatusChangedHandler GameSaysStarted;
		event SaysStatusChangedHandler GameSaysCompleted;

		event GameSetStartedHandler GameSetStarted;
		event GameSetCompletedHandler GameSetCompleted;
		int PlayerDealerNumber { get; }
		IGameHandler GameHandler { get; }
		int FirstPlayer { get; }
		List<List<IExplorationStatus>> Sets { get; }

		void Start();
		int GamePoints(int team);
	}
}