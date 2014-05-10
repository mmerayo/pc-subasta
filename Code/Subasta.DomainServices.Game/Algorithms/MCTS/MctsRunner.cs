using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using Subasta.DomainServices.Game.Algorithms.MCTS.Diagnostics;
using log4net;
using StructureMap;
using Subasta.ApplicationServices.Extensions;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.Domain.Game.Algorithms;

namespace Subasta.DomainServices.Game.Algorithms.MCTS
{
	class MctsRunner : IMctsRunner,IDisposable
	{
		private static readonly ILog Logger = LogManager.GetLogger(typeof (MctsRunner));

		private readonly IApplicationEventsExecutor _eventsExecutor;
		private readonly IMctsDiagnostics _diagnostics;
		private TreeNode _root;
		private const int MaxNumberExplorations = 120000; //to preserve memory
		private readonly object _rootLocker=new object();
		private CancellationTokenSource _tokenSource ;
		private Task _task;

		public MctsRunner(IApplicationEventsExecutor eventsExecutor,IMctsDiagnostics diagnostics)
		{
			_eventsExecutor = eventsExecutor;
			_diagnostics = diagnostics;
		}

		public void Start(IExplorationStatus status, object root=null)
		{
			Reset();
			//TODO: THE LOGICAL COMPLETE TO BE BY DEFAULT
			IExplorationStatus explorationStatus = status.Clone();
			explorationStatus.LogicalComplete = false;
			if (root == null)
			{
				_root = ObjectFactory.GetInstance<TreeNode>();
				_root.Initialize(explorationStatus);
			}
			else
			{
				_root = (TreeNode) root;
			}
			CompactMemory();

			_tokenSource = new CancellationTokenSource();
			_task = Task.Factory.StartNew(() =>
			                              {
										  CancellationToken ct = _tokenSource.Token;
			                              	Thread.CurrentThread.Name = "MctsRunner";
			                              	int count = 0;
			                              	while (_root.GetNodeInfo(1).NumberVisits <= MaxNumberExplorations)
			                              	{
			                              		if (ct.IsCancellationRequested)
			                              		{
			                              			break;
			                              		}
			                              		try
			                              		{
													//using (var mfp = new MemoryFailPoint(1))
													//{
													    lock (_rootLocker)
			                              				{
			                              					if (_root == null)
			                              						return;
			                              					_root.Select((++count%2) + 1);

			                              				}
			                              			//}
			                              		}
			                              		catch (InsufficientMemoryException ex)
			                              		{
			                              			Logger.Error("Start",ex);
			                              		}
			                              		catch (NullReferenceException ex)
			                              		{
													Logger.Error("Start", ex);
			                              		}
			                              		catch(Exception ex)
			                              		{
													GC.Collect(3, GCCollectionMode.Forced);

													Logger.Error("Start", ex);
			                              		}
			                              	}
			                              	GC.Collect(1, GCCollectionMode.Optimized);
			                              }, _tokenSource.Token).LogTaskException(Logger);

		}

		private static void CompactMemory()
		{
			GC.Collect(3, GCCollectionMode.Forced);
			try
			{
				using (var mfp = new MemoryFailPoint(1536)) ;
			}
			catch (InsufficientMemoryException ex)
			{
				GC.Collect(3, GCCollectionMode.Forced);
				Logger.Error("Start", ex);
			}
		}

		public void Reset()
		{
			if (_tokenSource != null)
			{
				_tokenSource.Cancel();
				try
				{
					_task.Wait(TimeSpan.FromSeconds(20));
				}
				catch (Exception ex)
				{
					//TODO:log
				}
				finally
				{
					_task.Dispose();
					_task = null;
					_tokenSource.Dispose();
					_tokenSource = null;

				}
			}

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
			    ITreeNodeInfo treeNodeInfo = current.GetNodeInfo(turnTeam);

			    int previousVisits = int.MinValue;
			    int repetitions = 0;
			    while (treeNodeInfo.NumberVisits < 30000)
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
			_diagnostics.NodeStatus(current, bestChild, currentStatus.Turn);

			var result = new NodeResult(bestChild.ExplorationStatus);
			return result;
		}

		public int MaxDepth { get; set; }


		private TreeNode IterateToCurrentPrunning(IExplorationStatus currentStatus)
		{
			bool prune = currentStatus.LastCompletedHand != null &&
						 !currentStatus.LastCompletedHand.Declaration.HasValue;
			TreeNode current = _root;
			bool found = false;
			foreach (var hand in currentStatus.Hands)
			{
				var cardsByPlaySequence = hand.CardsByPlaySequence();
				int cardNum = 0;
				foreach (var card in cardsByPlaySequence)
				{
					if (card == null)
					{
						found = true;
						break;
					};
					//prunes those paths that have been passed so they are not used in future navigations
					EnsureNodeIsExpanded(currentStatus.TurnTeam,current);
					bool checkDeclarationPath = ++cardNum == 4;
					//only prunnes when tehre are not declarations
					if(prune)
					{
						DisposeObsoleteTreeItems(hand, card, current, checkDeclarationPath);
						current = current.Children.Single();
					}
					else
					{
						
						current = current.Children.Single(
							x =>
							(
								Equals(x.CardPlayed,card) && (!checkDeclarationPath ||
								(x.DeclarationPlayed == hand.Declaration))
							));
					}
				}
				if(found) break;
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