using System;
using Subasta.Domain.DalModels;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.Client.Common.Game
{
	public delegate void StatusChangedHandler(IExplorationStatus status);
	public delegate void SaysStatusChangedHandler(ISaysStatus status);
	public delegate void TurnChangedHandler(int turn);
	public interface IGameHandler
	{
		bool IsFinished { get; set; }
		IPlayer Player1 { get; }
		IPlayer Player2 { get; }
		IPlayer Player3 { get; }
		IPlayer Player4 { get; }
		int FirstPlayer { get; }
		ISuit Trump { get; }
		int TeamBets { get; }
		IExplorationStatus Status { get; }
		void Start();
		void Load(StoredGameData storedGame);
		void ConfigureNewGame(NewGameConfiguration gameConfiguration);
		event GamePlayerDeclaration DeclarationEmit;

		event StatusChangedHandler GameStarted;
		event StatusChangedHandler GameCompleted;
		event StatusChangedHandler GameStatusChanged;
		event SaysStatusChangedHandler GameSaysStatusChanged;
		event SaysStatusChangedHandler GameSaysStarted;
		event SaysStatusChangedHandler GameSaysCompleted;
		event MoveSelectionNeeded HumanPlayerMoveSelectionNeeded;
		event DeclarationSelectionNeeded HumanPlayerDeclarationSelectionNeeded;
		event SayNeededEvent HumanPlayerSayNeeded;
		event TrumpNeededEvent HumanPlayerTrumpNeeded;
		event GamePlayerPetaHandler GamePlayerPeta;

		IPlayer GetPlayer(int playerNum);
		event StatusChangedHandler HandCompleted;
		event TurnChangedHandler TurnChanged;

	}
}