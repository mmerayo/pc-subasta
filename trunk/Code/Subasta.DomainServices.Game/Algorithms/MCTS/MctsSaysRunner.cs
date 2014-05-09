using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using StructureMap;
using Subasta.ApplicationServices.Extensions;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.Domain.Game.Analysis;
using Subasta.Infrastructure.Domain;

namespace Subasta.DomainServices.Game.Algorithms.MCTS
{
	internal class MctsSaysRunner : IMctsSaysRunner, IDisposable
	{
		private static readonly ILog Logger = LogManager.GetLogger(typeof(MctsSaysRunner));
		private const int ROOT_OROS = 0;
		private const int ROOT_COPAS = 1;
		private const int ROOT_ESPADAS = 2;
		private const int ROOT_BASTOS = 3;
		private const int MaxNumberExplorations = 60000; //to preserve memory
		private readonly IApplicationEventsExecutor _eventsExecutor;
		private readonly ISaysExplorationListener _explorationListener;
		private readonly object _rootLocker = new object();

		private readonly List<Task> _tasks = new List<Task>();
		private TreeNode[] _roots;
		private CancellationTokenSource _tokenSource;

		public MctsSaysRunner(IApplicationEventsExecutor eventsExecutor, ISaysExplorationListener explorationListener)
		{
			_eventsExecutor = eventsExecutor;
			_explorationListener = explorationListener;
		}

		#region IDisposable Members

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

		#region IMctsSaysRunner Members

		public void Start(ISaysStatus sourceStatus)
		{
			Reset(null);
			_roots = new[]
			         {
			         	ObjectFactory.GetInstance<TreeNode>(),
			         	ObjectFactory.GetInstance<TreeNode>(),
			         	ObjectFactory.GetInstance<TreeNode>(),
			         	ObjectFactory.GetInstance<TreeNode>()
			         };
			_roots[ROOT_OROS].Initialize(sourceStatus.ExplorationStatusForOros());
			_roots[ROOT_COPAS].Initialize(sourceStatus.ExplorationStatusForCopas());
			_roots[ROOT_ESPADAS].Initialize(sourceStatus.ExplorationStatusForEspadas());
			_roots[ROOT_BASTOS].Initialize(sourceStatus.ExplorationStatusForBastos());

			StartExploration();
		}

		protected virtual void StartExploration()
		{
			_tokenSource = new CancellationTokenSource();
			CancellationToken ct = _tokenSource.Token;

			if (Environment.ProcessorCount <= 2)
			{
				StartExploration(new[] {ROOT_OROS, ROOT_COPAS, ROOT_ESPADAS, ROOT_BASTOS}, ct);
			}
			else if (Environment.ProcessorCount <= 4)
				{
				StartExploration(new[] { ROOT_OROS, ROOT_COPAS }, ct);
				StartExploration(new[] { ROOT_ESPADAS, ROOT_BASTOS }, ct);
				}
			else
			{
				StartExploration(new []{ROOT_OROS}, ct);
				StartExploration(new []{ROOT_COPAS}, ct);
				StartExploration(new []{ROOT_ESPADAS}, ct);
				StartExploration(new []{ROOT_BASTOS}, ct);
			}
		}

		public void Reset(object rootToNotToReset)
		{
			if (_tokenSource != null)
			{
				_tokenSource.Cancel();

				foreach (Task task in _tasks)
				{
					try
					{
						task.Wait(TimeSpan.FromSeconds(20));
						task.Dispose();
					}
					catch(Exception ex)
					{
						Logger.Error("Reset", ex);
					}
				}
				_tasks.Clear();
				_tokenSource.Dispose();
				_tokenSource = null;
			}
			var avoidRoot = rootToNotToReset as TreeNode;

			if (_roots != null)
				lock (_rootLocker)
					if (_roots != null)
					{


						IEnumerable<TreeNode> treeNodes = _roots.Where(x => x != avoidRoot);
						foreach (TreeNode root in treeNodes)
						{
							root.Dispose();
						}
						_roots = null;
					}
		}

		public ISuit ChooseTrump(int teamNumber)
		{
			return _roots.OrderByDescending(x => x.GetNodeInfo(teamNumber).AvgPoints).First().ExplorationStatus.Trump;
		}

		public object GetRoot(ISuit suit)
		{
			return _roots.Single(x => x.ExplorationStatus.Trump == suit);
		}

		public byte GetMaxExplorationFor(byte teamNumber, int minNumberExplorations, float maxRiskPercentage,TimeSpan timeLimit)
		{
			if (minNumberExplorations > MaxNumberExplorations)
				minNumberExplorations = MaxNumberExplorations;
			//ensure explorations
			DateTime limit = DateTime.UtcNow.Add(timeLimit);
			while (DateTime.UtcNow <= limit && _roots.Any(x => x.GetNodeInfo(teamNumber).NumberVisits < minNumberExplorations))
			{
				Thread.Sleep(250);
				_eventsExecutor.Execute();
			}


			//gets the max points in all suits while the percentage of success is higher than the specified
			IEnumerable<int> candidates =
				_roots.Select(x => x.GetNodeInfo(teamNumber).GetMaxPointsWithMinimumChances(maxRiskPercentage));
			return (byte) candidates.Max();

			//return
			//    (int)
			//        Math.Truncate(
			//            _roots.Select(x => x.GetNodeInfo(teamNumber).AvgPoints).OrderByDescending(x => x).First());
		}

		public void UpdateExplorationListeners()
		{
			_explorationListener.Update(Suit.FromId('O'), _roots[ROOT_OROS].GetNodeInfo(1), _roots[ROOT_OROS].GetNodeInfo(2));
			_explorationListener.Update(Suit.FromId('C'), _roots[ROOT_COPAS].GetNodeInfo(1), _roots[ROOT_COPAS].GetNodeInfo(2));
			_explorationListener.Update(Suit.FromId('E'), _roots[ROOT_ESPADAS].GetNodeInfo(1),
			                            _roots[ROOT_ESPADAS].GetNodeInfo(2));
			_explorationListener.Update(Suit.FromId('B'), _roots[ROOT_BASTOS].GetNodeInfo(1), _roots[ROOT_BASTOS].GetNodeInfo(2));
		}

		#endregion

		private void StartExploration(int[] rootIdxs, CancellationToken ct)
		{
			CompactMemory();
			var t = Task.Factory.StartNew(() =>
			                              {
											var roots=new List<TreeNode>(rootIdxs.Length);
			                              	roots.AddRange(rootIdxs.Select(idx => _roots[idx]));

			                              	string threadName=string.Join("_", roots.Select(x => x.ExplorationStatus.Trump.Name));

			                              	Thread.CurrentThread.Name = string.Format("MctsSaysRunner - {0}",threadName);
			                              	int count = 0;
			                              	//while not enough explorations
			                              	while (roots[0].GetNodeInfo(1).NumberVisits <= MaxNumberExplorations)
			                              	{
			                              		if (ct.IsCancellationRequested)
			                              		{
			                              			break;
			                              		}

			                              		try
			                              		{
			                              			using (var mfp = new MemoryFailPoint(4))
			                              			{
			                              				if (_roots == null)
			                              					return;
			                              				//Debug.WriteLine("rootIdx: {0} - Visits: team1: {1}, team2: {2}",rootIdx,root.GetNodeInfo(1).NumberVisits,root.GetNodeInfo(2).NumberVisits);

														roots.ForEach(x => x.Select((++count % 2) + 1));
			                              			}
			                              		}
			                              		catch (InsufficientMemoryException ex)
			                              		{
													GC.Collect(3,GCCollectionMode.Forced);
			                              			Logger.Error("StartExploration", ex);
			                              		}
			                              		catch (NullReferenceException ex)
			                              		{
			                              			Logger.Error("StartExploration", ex);
			                              		}
			                              		catch (Exception ex)
			                              		{
			                              			Logger.Error("StartExploration", ex);
			                              		}
			                              	}

			                              	//release the root branches as we just need the root info
			                              	roots.ForEach(x =>
			                              	              {
			                              	              	x.Children.ForEach(y => y.Dispose());
			                              	              	x.Children.Clear();
			                              	              });
			                              	GC.Collect(1, GCCollectionMode.Optimized);
			                              }, _tokenSource.Token).LogTaskException(Logger);
			_tasks.Add(t);
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


		private void Dispose(bool disposing)
		{
			Reset(null);
		}

		~MctsSaysRunner()
		{
			Dispose(false);
		}
	}
}