using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		private readonly object _rootLocker = new object();
		private TreeNode[] _roots;
		private IApplicationEventsExecutor _eventsExecutor;
		private readonly ISaysExplorationListener _explorationListener;

		public MctsSaysRunner(IApplicationEventsExecutor eventsExecutor,ISaysExplorationListener explorationListener)
		{
			_eventsExecutor = eventsExecutor;
			_explorationListener = explorationListener;
		}

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

			StartExploration(ROOT_OROS);
			StartExploration(ROOT_COPAS);
			StartExploration(ROOT_ESPADAS);
			StartExploration(ROOT_BASTOS);

		}

		private void StartExploration(int rootIdx)
		{

			Task.Factory.StartNew(() =>
								  {
								  TreeNode root = _roots[rootIdx];
								  Thread.CurrentThread.Name = string.Format("MctsSaysRunner - {0}", root.ExplorationStatus.Trump.Name);
									int count = 0;
									//while not enough explorations
									while (root.GetNodeInfo(1).NumberVisits<120000)
									{
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
								  });
		}

		public void Reset(object rootToNotToReset)
		{
			TreeNode avoidRoot = rootToNotToReset as TreeNode;

			if (_roots != null)
				lock (_rootLocker)
					if (_roots != null)
					{
						IEnumerable<TreeNode> treeNodes = _roots.Where(x => x != avoidRoot);
						foreach (var root in treeNodes)
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

		public int GetMaxExplorationFor(int teamNumber, int minNumberExplorations, double maxRiskPercentage)
		{
			//ensure explorations
			DateTime limit = DateTime.UtcNow.AddSeconds(5);
			while(DateTime.UtcNow<=limit && _roots.Any(x=>x.GetNodeInfo(teamNumber).NumberVisits<minNumberExplorations))
			{
				Thread.Sleep(250);
				_eventsExecutor.Execute();
			}


			//gets the max points in all suits while the percentage of success is higher than the specified
			IEnumerable<int> candidates = _roots.Select(x => x.GetNodeInfo(teamNumber).GetMaxPointsWithMinimumChances(maxRiskPercentage));
			return candidates.Max();

			//return
			//    (int)
			//        Math.Truncate(
			//            _roots.Select(x => x.GetNodeInfo(teamNumber).AvgPoints).OrderByDescending(x => x).First());
		}

		public void UpdateExplorationListeners()
		{
		_explorationListener.Update(Suit.FromId('O'), _roots[ROOT_OROS].GetNodeInfo(1), _roots[ROOT_OROS].GetNodeInfo(2));
			_explorationListener.Update(Suit.FromId('C'), _roots[ROOT_COPAS].GetNodeInfo(1), _roots[ROOT_COPAS].GetNodeInfo(2));
			_explorationListener.Update(Suit.FromId('E'), _roots[ROOT_ESPADAS].GetNodeInfo(1), _roots[ROOT_ESPADAS].GetNodeInfo(2));
			_explorationListener.Update(Suit.FromId('B'), _roots[ROOT_BASTOS].GetNodeInfo(1), _roots[ROOT_BASTOS].GetNodeInfo(2));
		}


		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
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