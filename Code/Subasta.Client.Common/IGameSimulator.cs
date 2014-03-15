using Subasta.Client.Common.Games;
using Subasta.Domain.Game;

namespace Subasta.Client.Common
{
	public delegate void StatusChangedHandler(IExplorationStatus status);
	public delegate string InputRequestedHandler();
	
	public interface IGameSimulator
	{
		bool IsFinished { get; set; }
		IPlayer Player1 { get; }
		IPlayer Player2 { get; }
		IPlayer Player3 { get; }
		IPlayer Player4 { get; }
		void NextMove();
		event StatusChangedHandler GameStatusChanged;
		event InputRequestedHandler InputRequested;
		void Start(int depth);
		void Load(StoredGameData storedGame);
	}
}