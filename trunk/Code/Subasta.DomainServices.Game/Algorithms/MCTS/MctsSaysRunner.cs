using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using StructureMap;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
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


		public void Start(ISaysStatus sourceStatus)
		{
			Reset();
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

									int count = 0;
									while (true)
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

		public void Reset()
		{
			if (_roots != null)
				lock (_rootLocker)
					if (_roots != null)
					{
						foreach (var root in _roots)
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

		public int GetMaxExplorationFor(int turnTeam)
		{
			while(_roots.Any(x=>x.GetNodeInfo(turnTeam).NumberVisits<10000))
				Thread.Sleep(250);

			return
				(int)
					Math.Truncate(
						_roots.Select(x => x.GetNodeInfo(turnTeam).AvgPoints).OrderByDescending(x => x).First());
		}


		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void Dispose(bool disposing)
		{
			Reset();
			
		}
		~MctsSaysRunner()
		{
			Dispose(false);
		}
	}
}