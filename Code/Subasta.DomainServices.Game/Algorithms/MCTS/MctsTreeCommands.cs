using System;
using System.Collections.Generic;
using System.Linq;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.MCTS
{
	class MctsTreeCommands : IMctsTreeCommands
	{
		/// <summary>
		/// gets the best found and prunes the passed non needed children
		/// </summary>
		/// <param name="currentStatus"></param>
		/// <returns></returns>
		public NodeResult GetBestFoundAndShallow(IExplorationStatus currentStatus)
		{
			DoSimulation(currentStatus);

			var current = IterateToCurrentPrunning(currentStatus);
			TreeNode selectBestChild = current.SelectBestChild();
			return new NodeResult(selectBestChild.ExplorationStatus);
		}

		private static void DoSimulation(IExplorationStatus currentStatus)
		{
			var current = TreeNode.Root(currentStatus.TurnTeam);
			DateTime limit = DateTime.UtcNow.Add(TimeSpan.FromSeconds(5));
			do
			{
				current.Select();
			} while (DateTime.UtcNow <= limit);
		}

		private static TreeNode IterateToCurrentPrunning(IExplorationStatus currentStatus)
		{
			TreeNode current = TreeNode.Root(currentStatus.TurnTeam);
			foreach (var hand in currentStatus.Hands)
			{
				foreach (var card in hand.CardsByPlaySequence())
				{
					if (card == null) return current;
					//prunes those paths that have been passed so they are not used in future navigations

					var treeNodes = current.Children.Where(x => !Equals(x.CardPlayed, card) || x.DeclarationPlayed != hand.Declaration);
					foreach (var treeNode in treeNodes)
					{
						current.Children.Remove(treeNode);
						treeNode.Dispose();
					}

					current = current.Children.Single();
				}
			}
			GC.Collect(3,GCCollectionMode.Optimized);
			return current;
		}
	}
}