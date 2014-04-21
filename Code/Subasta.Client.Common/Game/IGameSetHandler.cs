using Subasta.Domain.Game;

namespace Subasta.Client.Common.Game
{

	public delegate void GameSetStartedHandler();
	public delegate void GameSetCompletedHandler();
	//PARTIDA
	public interface IGameSetHandler
	{
		event StatusChangedHandler GameStarted;
		event StatusChangedHandler GameCompleted;
		
		event SaysStatusChangedHandler GameSaysStarted;
		event SaysStatusChangedHandler GameSaysCompleted;

		event GameSetStartedHandler GameSetStarted;
		event GameSetCompletedHandler GameSetCompleted;

		void Start();
	}
}