﻿using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.MCTS
{
	internal interface IMctsRunner
	{
		/// <summary>
		/// it starts the MCTS
		/// </summary>
		/// <param name="result"></param>
		void Start(IExplorationStatus result);
	}
}