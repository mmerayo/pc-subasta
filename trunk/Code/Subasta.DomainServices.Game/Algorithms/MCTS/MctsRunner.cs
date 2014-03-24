using System;
using System.Linq;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using StructureMap;
using Subasta.ApplicationServices;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.MCTS
{
	class MctsRunner : IMctsRunner,IDisposable
	{
		private bool _stop = false;
		private readonly IApplicationEventsExecutor _eventsExecutor;
		private TreeNode _root;

		public MctsRunner(IApplicationEventsExecutor eventsExecutor)
		{
			_eventsExecutor = eventsExecutor;
			
		}

		public void Start(int forTeamNumber,IExplorationStatus status)
		{
			if(_root!=null)
				_root.Dispose();

			_root = ObjectFactory.GetInstance<TreeNode>();
			_root.Initialize(forTeamNumber, status);
			_stop = false;
			Task.Factory.StartNew(() =>
				{
					while (!_stop)
					{
						DoSimulation();
					}
				});
		}


		private void DoSimulation()
		{
			if (_paused)
			{
				Thread.Sleep(250);
				return;
			}
			var current = _root;
			//DateTime limit = DateTime.UtcNow.Add(TimeSpan.FromSeconds(7));
			try
			{
				using (var mfp = new MemoryFailPoint(32))
				{
					int i = 0;
					const int threads = 32;
					var tasks = new Task[threads];
						for (int j = 0; j < threads; j++)
							tasks[j] = Task.Factory.StartNew(current.Select);

						if (++i % 20 == 0) _eventsExecutor.Execute();
						Task.WaitAll(tasks);

				}
			}
			catch (InsufficientMemoryException)
			{
				GC.Collect(3, GCCollectionMode.Forced);
				//log it
			}
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

		public void Stop()
		{
			_stop = true;
			if(_root!=null)
				_root.Dispose();
		}

		private bool _paused = false;
		public void Pause()
		{
			_paused = true;
			Thread.Sleep(250);
		}

		public void Restart()
		{
			_paused = false;
		}


		/// <summary>
		/// gets the best found and prunes the passed non needed children
		/// </summary>
		/// <param name="currentStatus"></param>
		/// <returns></returns>
		public NodeResult GetBest(IExplorationStatus currentStatus)
		{
			//DoSimulation(currentStatus);

			//Pause();
			var current = IterateToCurrentPrunning(currentStatus);
			EnsureNodeIsExpanded(current);
			
			TreeNode bestChild;
			do
			{
				Thread.Sleep(150);
				bestChild = current.SelectBestChild();
			} while (bestChild.NumberVisits < 500); //TODO: LEVEL??
			var result= new NodeResult(bestChild.ExplorationStatus);
			//Restart();
			return result;
		}

		public int MaxDepth { get; set; }


		private TreeNode IterateToCurrentPrunning(IExplorationStatus currentStatus)
		{
			TreeNode current = _root;
			foreach (var hand in currentStatus.Hands)
			{
				var cardsByPlaySequence = hand.CardsByPlaySequence();
				foreach (var card in cardsByPlaySequence)
				{
					if (card == null) return current;
					//prunes those paths that have been passed so they are not used in future navigations
					EnsureNodeIsExpanded(current);
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

		private static void EnsureNodeIsExpanded(TreeNode current)
		{
			while (current.IsLeaf)
			{
				Thread.Sleep(100);
				//do nothing //TODO: expand explicitly?
			}
		}


		private bool _disposed = false;

		
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void Dispose(bool disposing)
		{

			Stop();
		}
		~MctsRunner()
		{
			Dispose(false);
		}
	}
}