﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Subasta.Domain.Deck;

namespace Subasta.Domain.Game
{
	public delegate void GameStatusChangedHandler(IExplorationStatus status);
	public delegate void GameSaysStatusChangedHandler(ISaysStatus status);

	public delegate void GamePlayerPetaHandler(IPlayer player, IExplorationStatus status);
	public delegate void GamePlayerDeclarationHandler(IPlayer player, Declaration declaration);

	public delegate void GameEndHandler(IExplorationStatus status);

	public interface IGame
	{
		IPlayer Player1 { get; }
		IPlayer Player2 { get; }
		IPlayer Player3 { get; }
		IPlayer Player4 { get; }
		IExplorationStatus Status { get; }

		event GameStatusChangedHandler GameStatusChanged;
		event GameStatusChangedHandler GameStarted;
		event GameStatusChangedHandler GameCompleted;
		event GameEndHandler GameEnded;
		
		
		event GameStatusChangedHandler HandCompleted;
		event GamePlayerPetaHandler GamePlayerPeta;
		event GamePlayerDeclarationHandler PlayerDeclarationEmitted;


		event GameSaysStatusChangedHandler GameSaysStatusChanged;
		event GameSaysStatusChangedHandler GameSaysStarted;
		event GameSaysStatusChangedHandler GameSaysCompleted;



		void StartGame();
		void SetGameInfo(IPlayer p1, IPlayer p2, IPlayer p3, IPlayer p4, int firstPlayer);
	}
}
