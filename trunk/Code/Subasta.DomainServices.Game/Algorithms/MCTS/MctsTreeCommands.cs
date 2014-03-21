using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading.Tasks;
using Subasta.ApplicationServices;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.MCTS
{
	class MctsTreeCommands : IMctsTreeCommands
	{
		private readonly IMctsRunner _runner;
		private readonly IApplicationEventsExecutor _eventsExecutor;

		public MctsTreeCommands(IMctsRunner runner,IApplicationEventsExecutor eventsExecutor)
		{
			_runner = runner;
			_eventsExecutor = eventsExecutor;
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

		private  void DoSimulation(IExplorationStatus currentStatus)
		{
			var current = TreeNode.Root(currentStatus.TurnTeam);
			DateTime limit = DateTime.UtcNow.Add(TimeSpan.FromSeconds(5));
			try
			{
				using (var mfp = new MemoryFailPoint(32))
				{
					int i = 0;
					const int threads = 16;
					var tasks = new Task[threads];
					do
					{
						for (int j = 0; j < threads;j++ )
							tasks[j]=Task.Factory.StartNew(current.Select);
						
						if(++i%20==0) _eventsExecutor.Execute();
						Task.WaitAll(tasks);

					} while (DateTime.UtcNow <= limit);
				}
			}
			catch (InsufficientMemoryException)
			{
				GC.Collect(3, GCCollectionMode.Forced);
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
			//GC.Collect(3,GCCollectionMode.Optimized);
			return current;
		}
	}
}