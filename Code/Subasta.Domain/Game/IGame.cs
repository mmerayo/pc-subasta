﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Subasta.Domain.Deck;

namespace Subasta.Domain.Game
{
	public delegate void GameStatusChangedHandler(IExplorationStatus status);

	public interface IGame
	{
		IPlayer Player1 { get; }
		IPlayer Player2 { get; }
		IPlayer Player3 { get; }
		IPlayer Player4 { get; }

		event GameStatusChangedHandler GameStatusChanged;
		event GameStatusChangedHandler GameStarted;
		event GameStatusChangedHandler GameCompleted;
		void StartGame();
		void SetGameInfo(IPlayer p1, IPlayer p2, IPlayer p3, IPlayer p4, int firstPlayer, int teamBets, ISuit trump, int pointsBet);
	}
}