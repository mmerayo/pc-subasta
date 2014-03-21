using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.MCTS
{
	class MctsTreeCommands : IMctsTreeCommands
	{
		private IMctsRunner _runner;

		public MctsTreeCommands(IMctsRunner runner)
		{
			_runner = runner;
		}

		/// <summary>
		/// gets the best found and prunes the passed non needed children
		/// </summary>
		/// <param name="currentStatus"></param>
		/// <returns></returns>
		public NodeResult GetBestFoundAndShallow(IExplorationStatus currentStatus)
		{
			DoSimulation(currentStatus);

			_runner.Pause();
			var current = IterateToCurrentPrunning(currentStatus);
			TreeNode selectBestChild = current.SelectBestChild();
			var result= new NodeResult(selectBestChild.ExplorationStatus);
			_runner.Restart();
			return result;
		}

		private static void DoSimulation(IExplorationStatus currentStatus)
		{
			var current = TreeNode.Root(currentStatus.TurnTeam);
			DateTime limit = DateTime.UtcNow.Add(TimeSpan.FromSeconds(10));
			try
			{
				using (var mfp = new MemoryFailPoint(16))
				{
					int i = 0;
					do
					{
						current.Select();
						if(++i%20==0) System.Windows.Forms.Application.DoEvents()
					} while (DateTime.UtcNow <= limit);
				}
			}
			catch (InsufficientMemoryException)
			{
				//log it
			} 
		}

		private static TreeNode IterateToCurrentPrunning(IExplorationStatus currentStatus)
		{
			TreeNode root = TreeNode.Root(currentStatus.TurnTeam);
			TreeNode current = root;
			foreach (var hand in currentStatus.Hands)
			{
				IEnumerable<ICard> cardsByPlaySequence = hand.CardsByPlaySequence();
				foreach (var card in cardsByPlaySequence)
				{
					if (card == null) return current;
					//prunes those paths that have been passed so they are not used in future navigations

					//while (!current.Children.Any(x => Equals(x.CardPlayed, card) && x.DeclarationPlayed == hand.Declaration))
					//{
					//    root.Select();
					//}
					//var treeNodes = current.Children.Where(x => !Equals(x.CardPlayed, card) || x.DeclarationPlayed != hand.Declaration).ToArray();
					var treeNodes = current.Children.Where(x => !Equals(x.CardPlayed, card)).ToArray();
					foreach (var treeNode in treeNodes)
					{
						treeNode.Dispose();
						current.Children.Remove(treeNode);
					}

					current = current.Children.Single();
				}
			}
			GC.Collect(3,GCCollectionMode.Optimized);
			return current;
		}
	}
}