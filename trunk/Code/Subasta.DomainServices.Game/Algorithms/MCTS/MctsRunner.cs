using System;
using System.Collections.Generic;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.MCTS
{
	class MctsRunner : IMctsRunner,IDisposable
	{
		public void Start(IExplorationStatus result)
		{
			//TreeNode<NodeInfo>.Initialize();

			//TODO: BY THREADS
		}

		/// <summary>
		///Starting at the root node, a child selection
		///policy is recursively applied to descend through
		///Starting at the root node, a child selection
		///policy is recursively applied to descend through
		/// </summary>
		private static void Select()
		{
			//TreeNode<NodeInfo>.Root.Select();// done by each thread
		}


		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}