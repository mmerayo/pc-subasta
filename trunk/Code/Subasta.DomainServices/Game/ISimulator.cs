﻿using System;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game
{
	
	public interface ISimulator 
	{
		NodeResult GetBest(IExplorationStatus currentStatus);

		int MaxDepth { get; set; }
		IPlayer Player { get; set; }

		void Start(int teamNumber, IExplorationStatus initialStatus);

	    void Reset();
	}
}