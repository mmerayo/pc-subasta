using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using StructureMap;
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

		public IPlayer Player { get; set; }

		public void Start(int forTeamNumber,IExplorationStatus status)
		{
			if(_root!=null)
				_root.Dispose();

			_root = ObjectFactory.GetInstance<TreeNode>();
			_root.Initialize(forTeamNumber, status);
			_stop = false;
			Task.Factory.StartNew(() =>
				{
					try
					{
						while (!_stop)
						{
							DoSimulation();
						}
					}
					catch (Exception ex)
					{
						throw;
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
				using (var mfp = new MemoryFailPoint(16))
				{
					int i = 0;
					const int threads = 8;
					var tasks = new Task[threads];
					for (int j = 0; j < threads; j++)
						tasks[j] = Task.Factory.StartNew(current.Select);

					if (++i%20 == 0) _eventsExecutor.Execute();
					Task.WaitAll(tasks);

				}
			}
			catch (InsufficientMemoryException)
			{
				GC.Collect(GC.MaxGeneration, GCCollectionMode.Optimized);
				//log it
			}
			catch
			{
				throw;
			}
		}

		public void Stop()
		{
			_stop = true;
			Thread.Sleep(150);
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
			DateTime limit = DateTime.UtcNow.Add(TimeSpan.FromSeconds(10));
			const int timesRepeated = 15;
			int repetitions = 0;
			TreeNode previousBest=null;
			do
			{
				
				bestChild = current.SelectBestChild();
				if (previousBest == null || !previousBest.CardPlayed.Equals(bestChild.CardPlayed))
				{
					previousBest = bestChild;
					repetitions = 0;
					
				}
				else
				{
					repetitions++;
					
				}
				_eventsExecutor.Execute();
				Thread.Sleep(250);
				Debug.WriteLine("{0} - Hand:{3} - {1} - Visits:{2}", Player.Name, bestChild.CardPlayed.ToShortString(), bestChild.NumberVisits,currentStatus.CurrentHand.Sequence);

			} while (current.Children.Count>1 && repetitions<timesRepeated && DateTime.UtcNow <= limit); //TODO: LEVEL??
			var result = new NodeResult(bestChild.ExplorationStatus);
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
			GC.Collect(GC.MaxGeneration, GCCollectionMode.Optimized);
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