using System;
using System.Threading;
using System.Threading.Tasks;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.MCTS
{
	class MctsRunner : IMctsRunner,IDisposable
	{
		private bool _stop = false;

		public void Start(IExplorationStatus status)
		{
			TreeNode.Initialize(status);

			//temp
			//TreeNode treeNode = TreeNode.Root(1);

			//for (int i = 1; i < 1000; i++)
			//    treeNode.Select();

			//Task.Factory.StartNew(() => Explore(1));
			//Task.Factory.StartNew(() => Explore(2));

		}

		private void Explore(int teamNumber)
		{
			int availableThreads = 1;
			do
			{
				while(availableThreads > 0)
				{
					Task.Factory.StartNew(() =>
						{
							var rootTeam = TreeNode.Root(teamNumber);
							availableThreads = DoSelect(availableThreads, rootTeam);
						});
				}
			} while (!_stop);
		}

		private static int DoSelect(int availableThreads, TreeNode rootTeam)
		{
			availableThreads--;
			rootTeam.Select();
			availableThreads++;
			return availableThreads;
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
			TreeNode.Reset();
		}
		private bool _disposed = false;
		public void Dispose()
		{
			TreeNode.Reset();
		}
		private void Dispose(bool disposing)
		{
			
		}
	}
}