using System;
using Subasta.Client.Common.Games;
using Subasta.Domain.DalModels;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.Client.Common
{
	public delegate void StatusChangedHandler(IExplorationStatus status,TimeSpan timeTaken);
	public delegate string InputRequestedHandler();
	public delegate void SaysStatusChangedHandler(ISaysStatus status);
	
	public interface IGameSimulator
	{
		bool IsFinished { get; set; }
		IPlayer Player1 { get; }
		IPlayer Player2 { get; }
		IPlayer Player3 { get; }
		IPlayer Player4 { get; }
		int FirstPlayer { get; }
		ISuit Trump { get; }
		int TeamBets { get; }
		event InputRequestedHandler InputRequested;
		void Start(int depth=int.MinValue);
		void Load(StoredGameData storedGame);
		event StatusChangedHandler GameStarted;
		event StatusChangedHandler GameCompleted;
		event StatusChangedHandler GameStatusChanged;
		event SaysStatusChangedHandler GameSaysStatusChanged;
		event SaysStatusChangedHandler GameSaysStarted;
		event SaysStatusChangedHandler GameSaysCompleted;

		event MoveSelectionNeeded HumanPlayerMoveSelectionNeeded;
		event DeclarationSelectionNeeded HumanPlayerDeclarationSelectionNeeded;
	}
}