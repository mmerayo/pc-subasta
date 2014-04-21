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

		void Start();
		int GamePoints(int team);
	}
}