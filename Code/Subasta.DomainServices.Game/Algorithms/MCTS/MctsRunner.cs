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
		private readonly IApplicationEventsExecutor _eventsExecutor;
		private TreeNode _root;
		private readonly object _rootLocker=new object();
		public MctsRunner(IApplicationEventsExecutor eventsExecutor)
		{
			_eventsExecutor = eventsExecutor;
			
		}
		
		public void Start(IExplorationStatus status)
		{
			Reset();
			
			_root = ObjectFactory.GetInstance<TreeNode>();
			_root.Initialize( status);

			Task.Factory.StartNew(() =>
			{
				int count = 0;
				while (true)
				{
					try
					{
						using (var mfp = new MemoryFailPoint(4))
						{
							lock (_rootLocker)
							{
								if(_root==null)
									return;
								_root.Select((++count % 2)+1);
								
							}
						}
					}
					catch (InsufficientMemoryException)
					{
						//log
					}
				}
			});

		}

		public void Reset()
		{
			if (_root != null)
				lock (_rootLocker)
					if (_root != null)
					{
						_root.Dispose();
						_root = null;
					}
		}


		/// <summary>
		/// gets the best found and prunes the passed non needed children
		/// </summary>
		/// <param name="currentStatus"></param>
		/// <returns></returns>
		public NodeResult GetBest(IExplorationStatus currentStatus)
		{

			int turnTeam = currentStatus.TurnTeam;

			var current = IterateToCurrentPrunning(currentStatus);
			EnsureNodeIsExpanded(turnTeam, current);

		    int selections = 0;
			if(current.Children.Count!=1)

			{
			    ITreeNodeInfo treeNodeInfo = _root.GetNodeInfo(turnTeam);
			    int previousVisits = int.MinValue;
			    int repetitions = 0;
			    while (treeNodeInfo.NumberVisits < 3000*currentStatus.TotalMoves)
			    {
			        if (previousVisits == treeNodeInfo.NumberVisits)
			            repetitions++;
			        else
			        {
			            repetitions = 0;
			            previousVisits = treeNodeInfo.NumberVisits;
			        }
			        if (repetitions == 10)
			            break;
			        if (++selections%100 == 0)
			            _eventsExecutor.Execute();
			    }
			}
		    TreeNode bestChild = current.SelectBestMove(turnTeam);
			
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
					EnsureNodeIsExpanded(currentStatus.TurnTeam,current);
					
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

		private void EnsureNodeIsExpanded(int turnTeam, TreeNode current)
		{
			while (current.IsLeaf)
			{
				current.Select(turnTeam);
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
			Reset();
			
		}
		~MctsRunner()
		{
			Dispose(false);
		}
	}
}