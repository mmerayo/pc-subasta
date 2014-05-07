using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using StructureMap;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.Domain.Game.Analysis;
using Subasta.Infrastructure.Domain;

namespace Subasta.DomainServices.Game.Algorithms.MCTS
{
	internal class MctsSaysRunner : IMctsSaysRunner, IDisposable
	{
		private const int ROOT_OROS = 0;
		private const int ROOT_COPAS = 1;
		private const int ROOT_ESPADAS = 2;
		private const int ROOT_BASTOS = 3;
		private const int MaxNumberExplorations = 120000; //to preserve memory
		private readonly IApplicationEventsExecutor _eventsExecutor;
		private readonly ISaysExplorationListener _explorationListener;
		private readonly object _rootLocker = new object();

		private readonly Task[] _tasks = new Task[4];
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

			_tokenSource = new CancellationTokenSource();
			CancellationToken ct = _tokenSource.Token;

			StartExploration(ROOT_OROS, ct);
			StartExploration(ROOT_COPAS, ct);
			StartExploration(ROOT_ESPADAS, ct);
			StartExploration(ROOT_BASTOS, ct);
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
					}
					catch
					{
					}
				}

				for (int index = 0; index < _tasks.Length; index++)
				{
					Task task = _tasks[index];
					task.Dispose();
					_tasks[index] = null;
				}
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

		public byte GetMaxExplorationFor(byte teamNumber, int minNumberExplorations, float maxRiskPercentage)
		{
			if (minNumberExplorations > MaxNumberExplorations)
				minNumberExplorations = MaxNumberExplorations;
			//ensure explorations
			DateTime limit = DateTime.UtcNow.AddSeconds(5);
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

		private void StartExploration(int rootIdx, CancellationToken ct)
		{
			_tasks[rootIdx] = Task.Factory.StartNew(() =>
			                                        {
			                                        	TreeNode root = _roots[rootIdx];
			                                        	Thread.CurrentThread.Name = string.Format("MctsSaysRunner - {0}",
			                                        	                                          root.ExplorationStatus.Trump.Name);
			                                        	int count = 0;
			                                        	//while not enough explorations
			                                        	while (root.GetNodeInfo(1).NumberVisits <= MaxNumberExplorations)
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
			                                        				root.Select((++count%2) + 1);
			                                        			}
			                                        		}
			                                        		catch (InsufficientMemoryException)
			                                        		{
			                                        			//log
			                                        		}
			                                        		catch (NullReferenceException)
			                                        		{
			                                        			return;
			                                        		}
			                                        	}

														//release the root branches as we just need the root info
														root.Children.ForEach(x=>x.Dispose());
														root.Children.Clear();
														GC.Collect(1, GCCollectionMode.Optimized);
			                                        }, _tokenSource.Token);
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