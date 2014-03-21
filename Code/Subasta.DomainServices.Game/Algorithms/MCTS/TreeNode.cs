using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading;
using StructureMap;
using Subasta.Domain;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.MCTS
{

	internal sealed class TreeNode:IDisposable
	{
		private static readonly Random _random = new Random((int) DateTime.UtcNow.Ticks);
		private static TreeNode _rootTeam1;
		private static TreeNode _rootTeam2;
		private const double Epsilon = 1e-6;

		private readonly int _teamNumber;
		private readonly INonFilteredCandidatesSelector _candidatesSelector;
		private readonly IExplorationStatus _explorationStatus;
		private readonly ICandidatePlayer _candidatePlayer;

		private readonly object _syncLock = new object();
		private ICard _cardPlayed;
		private Declaration? _declarationPlayed;
		private static readonly object _typeLock = new object();
		private bool _expanded;
		private List<TreeNode> _children;

		public static TreeNode Root(int teamNumber)
		{
			switch (teamNumber)
			{
				case 1:
					return RootTeam1;
				case 2:
					return RootTeam2;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private static TreeNode RootTeam1
		{
			get
			{
				lock (_typeLock)
					return _rootTeam1;
			}
			set
			{
				lock (_typeLock)
					_rootTeam1 = value;
			}
		}

		private static TreeNode RootTeam2
		{
			get { lock (_typeLock) return _rootTeam2; }
			set { lock (_typeLock) _rootTeam2 = value; }
		}

		public static void Initialize(IExplorationStatus status)
		{
			RootTeam1 = new TreeNode(1, ObjectFactory.GetInstance<INonFilteredCandidatesSelector>(), status,
			                         ObjectFactory.GetInstance<ICandidatePlayer>());
			RootTeam2 = new TreeNode(2, ObjectFactory.GetInstance<INonFilteredCandidatesSelector>(), status,
			                         ObjectFactory.GetInstance<ICandidatePlayer>());
		}


		private TreeNode(int teamNumber, INonFilteredCandidatesSelector candidatesSelector, IExplorationStatus explorationStatus,
		                 ICandidatePlayer candidatePlayer)
		{
			_teamNumber = teamNumber;
			_candidatesSelector = candidatesSelector;
			_explorationStatus = explorationStatus.Clone();
			_candidatePlayer = candidatePlayer;
		}

		private bool IsLeaf
		{
			get
			{
				lock (_syncLock)
					return !_expanded;
			}
		}

		private double TotalValue { get; set; }
		private int NumberVisits { get; set; }

		public List<TreeNode> Children
		{
			get
			{
				//lock (_syncLock)
				return _children;
			}
			private set
			{
				//if (_children == null)
				//    lock (_syncLock)
						if (_children == null)
							_children = value;
			}
		}

		public IExplorationStatus ExplorationStatus
		{
			get
			{
				return _explorationStatus;
			}
		}

		public ICard CardPlayed
		{
			get { return _cardPlayed; }
			private set {_cardPlayed = value; }
		}

		public Declaration? DeclarationPlayed
		{
			get { return _declarationPlayed; }
			private set { _declarationPlayed = value; }
		}
		
		public void Select()
		{
			lock (_syncLock)
			{
				try
				{
					using (var mfp = new MemoryFailPoint(32))
					{
						var visited = new List<TreeNode>();
						var current = this;
						visited.Add(current);

						while (!current.IsLeaf)
						{
							current = current.SelectBestChild();
							visited.Add(current);
						}
						current.Expand();
						var newNode = current.SelectBestChild();
						//if (newNode == null) return; //this is due a bug in the algorithm
						visited.Add(newNode);
						var simulationValue = GetSimulationValue(newNode);

						foreach (var treeNode in visited)
						{
							treeNode.UpdateStatus(simulationValue);
						}
					}
				}
				catch (InsufficientMemoryException)
				{
					GC.Collect(3, GCCollectionMode.Forced);
					//log it
				}
				catch (ObjectDisposedException) //it was being disposed while doing the select
				{
					//log
				}
				//catch (Exception ex)
				//{
				//    //swallows
				//    Debug.WriteLine(string.Format("Select ") ex);
				//}
			}
		}

		private void Expand()
		{
			if (!_expanded)
				lock (_syncLock)
					if (!_expanded)
					{
						DoExpand();
						_expanded = true;
					}
		}

		private void DoExpand()
		{
			ICard[] candidates = _candidatesSelector.GetCandidates(ExplorationStatus, ExplorationStatus.Turn);
			Children = new List<TreeNode>();
			foreach (var candidate in candidates)
			{
				var newStatus = _candidatePlayer.PlayCandidate(ExplorationStatus, ExplorationStatus.Turn, candidate);

				if (newStatus.CurrentHand.IsEmpty && newStatus.Declarables.Length > 0)
				{
					var declarations = newStatus.Declarables;

					foreach (var declaration in declarations)
					{
						newStatus.LastCompletedHand.SetDeclaration(declaration);
						Children.Add(new TreeNode(_teamNumber, _candidatesSelector, newStatus, _candidatePlayer)
							{
								CardPlayed = candidate,
								DeclarationPlayed = declaration
							});
					}
				}
				else
				{
					Children.Add(new TreeNode(_teamNumber, _candidatesSelector, newStatus, _candidatePlayer)
						{
							CardPlayed = candidate
						});
				}
			}
		}

		public TreeNode SelectBestChild()
		{
			lock (_syncLock)
			{
				TreeNode selected = null;
				double bestValue = double.MinValue;

				var treeNodes = Children.ToArray();
				foreach (var treeNode in treeNodes)
				{
					double uctValue = treeNode.TotalValue/(treeNode.NumberVisits + Epsilon) +
					                  Math.Sqrt(Math.Log(NumberVisits + 1)/(treeNode.NumberVisits + Epsilon)) +
					                  _random.NextDouble()*Epsilon;
					if (uctValue > bestValue)
					{
						selected = treeNode;
						bestValue = uctValue;
					}
				}
				if (selected == null)
				{
					lock (_syncLock)
						if (_disposed)
							throw new ObjectDisposedException("The node was disposed, prevent caller for this situation");
					else
					    throw new NotImplementedException("Unknown. not implemented situation");
				}

				return selected;
			}
		}

		//simulation
		//returns 0(loss) or 1(win)
		private double GetSimulationValue(TreeNode node)
		{
			var currentStatus = node.ExplorationStatus.Clone();

			while (!currentStatus.IsCompleted)
			{
				ICard[] candidates = _candidatesSelector.GetCandidates(currentStatus, currentStatus.Turn);
				int index = _random.Next(0, candidates.Length - 1);
				currentStatus = _candidatePlayer.PlayCandidate(currentStatus, currentStatus.Turn, candidates[index]);

				//Add the most valuable declaration
				if (currentStatus.CurrentHand.IsEmpty)
				{
					var declarations = currentStatus.Declarables;
					if (declarations != null && declarations.Length > 0)
					{
						Declaration declaration = declarations.OrderByDescending(DeclarationValues.ValueOf).First();
						currentStatus.LastCompletedHand.SetDeclaration(declaration);
					}
				}
			}

			if (currentStatus.TeamWinner == _teamNumber)
				return 1;
			return 0;
		}

		private void UpdateStatus(double value)
		{
			lock (_syncLock)
			{
				NumberVisits++;
				TotalValue += value;
			}
		}

		public static void Reset()
		{
			RootTeam1.Dispose();
			RootTeam1 = null;
			RootTeam2.Dispose();
			RootTeam2 = null;
		}

		private bool _disposed = false;
		public void Dispose()
		{
			Dispose(true);
		}

		private void Dispose(bool disposing)
		{
			if (Children != null)
			{
				Children.ForEach(x => x.Dispose());
				Children.Clear();
			}
			lock (_syncLock)
				_disposed = true;
		}

		~TreeNode()
		{
			Dispose(false);
		}
	}
}