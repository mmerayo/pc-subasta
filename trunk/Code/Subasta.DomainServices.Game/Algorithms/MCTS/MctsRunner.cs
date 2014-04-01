using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using StructureMap;
using Subasta.Domain.Deck;
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
			int selections = 0;
			if(current.Children.Count>1)
				while (++selections < 3000)
				{
					//DoSimulation();
					try
					{
						using (var mfp = new MemoryFailPoint(4))
						{
							current.Select();
						}
					}
					catch (InsufficientMemoryException)
					{
						//log
					}
					if (selections % 100 == 0) _eventsExecutor.Execute();
				}
			bestChild = current.SelectBestMove();
			
			var result = new NodeResult(bestChild.ExplorationStatus);
			return result;
		}

		public int MaxDepth { get; set; }


		private TreeNode IterateToCurrentPrunning(IExplorationStatus currentStatus)
		{
			TreeNode current = _root;
			foreach (var hand in currentStatus.Hands)
			{
				var cardsByPlaySequence = hand.CardsByPlaySequence();
				int cardNum = 0;
				foreach (var card in cardsByPlaySequence)
				{
					if (card == null) return current;
					//prunes those paths that have been passed so they are not used in future navigations
					EnsureNodeIsExpanded(current);
					
					DisposeObsoleteTreeItems(hand,card, current, ++cardNum==4);
					
					current = current.Children.Single();
				}
			}
			return current;
		}

		private static void DisposeObsoleteTreeItems(IHand hand,ICard card, TreeNode currentNode, bool checkDeclarationPath )
		{
			Func<TreeNode, bool> predicate;

			if (checkDeclarationPath && hand.Declaration.HasValue)
			{
				predicate = x => !Equals(x.CardPlayed, card) || hand.Declaration != x.DeclarationPlayed;
			}
			else
			{
				predicate = x => !Equals(x.CardPlayed, card);
			}
			var treeNodes = currentNode.Children.Where(predicate).ToArray();

			foreach (var treeNode in treeNodes)
			{
				treeNode.Dispose();
				currentNode.Children.Remove(treeNode);
			}
		}

		private void EnsureNodeIsExpanded(TreeNode current)
		{
			while (current.IsLeaf)
			{
				current.Select();
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

			if (_root != null)
			{
				_root.Dispose();
				_root = null;
			}
		}
		~MctsRunner()
		{
			Dispose(false);
		}
	}
}