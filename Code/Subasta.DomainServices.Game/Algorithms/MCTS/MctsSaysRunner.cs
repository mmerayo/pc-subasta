using System;
using System.Linq;
using System.Runtime;
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
			                      	int count = 0;
			                      	while (true)
			                      	{
			                      		try
			                      		{
			                      			using (var mfp = new MemoryFailPoint(4))
			                      			{

			                      				if (_roots == null)
			                      					return;
			                      				_roots[rootIdx].Select((++count%2) + 1);


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

		public SayKind GetSay(ISaysStatus saysStatus)
		{
			return SayKind.As;//TODO: THIS AND THE INTERPRETER
		}

		public ISuit ChooseTrump(int teamNumber)
		{
			return _roots.OrderByDescending(x => x.GetNodeInfo(teamNumber).Coeficient).First().ExplorationStatus.Trump;
		}

		public object GetRoot(ISuit suit)
		{
			return _roots.Single(x => x.ExplorationStatus.Trump == suit);
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