using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using StructureMap;
using Subasta.Domain;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.MCTS
{

	internal sealed class TreeNode
	{
		private static readonly Random _random = new Random((int) DateTime.UtcNow.Ticks);
		private static TreeNode _rootTeam1;
		private static TreeNode _rootTeam2;
		private const double Epsilon = 1e-6;

		private readonly int _teamNumber;
		private readonly INonFilteredCandidatesSelector _candidatesSelector;
		private readonly IExplorationStatus _status;
		private readonly ICandidatePlayer _candidatePlayer;

		private readonly object _syncLock = new object();
		private static readonly object _typeLock = new object();

		public static TreeNode RootTeam1
		{
			get
			{
				lock (_typeLock)
					return _rootTeam1;
			}
			private set
			{
				lock (_typeLock)
					_rootTeam1 = value;
			}
		}

		public static TreeNode RootTeam2
		{
			get { lock (_typeLock) return _rootTeam2; }
			private set { lock (_typeLock) _rootTeam2 = value; }
		}

		public static void Initialize(IExplorationStatus status)
		{
			RootTeam1 = new TreeNode(1, ObjectFactory.GetInstance<INonFilteredCandidatesSelector>(), status,
			                         ObjectFactory.GetInstance<ICandidatePlayer>());
			RootTeam2 = new TreeNode(2, ObjectFactory.GetInstance<INonFilteredCandidatesSelector>(), status,
			                         ObjectFactory.GetInstance<ICandidatePlayer>());
		}


		private TreeNode(int teamNumber, INonFilteredCandidatesSelector candidatesSelector, IExplorationStatus status,
		                 ICandidatePlayer candidatePlayer)
		{
			_teamNumber = teamNumber;
			_candidatesSelector = candidatesSelector;
			_status = status;
			_candidatePlayer = candidatePlayer;
		}

		private bool IsLeaf
		{
			get { return Children == null; }
		}

		private double TotalValue { get; set; }
		private int NumberVisits { get; set; }

		public List<TreeNode> Children { get; private set; }


		public void Select()
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
			visited.Add(newNode);
			var simulationValue = GetSimulationValue(newNode);

			foreach (var treeNode in visited)
			{
				treeNode.UpdateStatus(simulationValue);
			}

		}

		private void Expand()
		{
			ICard[] candidates = _candidatesSelector.GetCandidates(_status, _status.Turn);
			Children = new List<TreeNode>();
			foreach (var candidate in candidates)
			{
				var newStatus = _candidatePlayer.PlayCandidate(_status, _status.Turn, candidate);

				if (newStatus.CurrentHand.IsEmpty)
				{
					var declarations = newStatus.Declarables;

					foreach (var declaration in declarations)
					{
						newStatus.LastCompletedHand.SetDeclaration(declaration);
						Children.Add(new TreeNode(_teamNumber, _candidatesSelector, newStatus, _candidatePlayer));
					}
				}
				else
				{
					Children.Add(new TreeNode(_teamNumber, _candidatesSelector, newStatus, _candidatePlayer));
				}
			}
		}

		private TreeNode SelectBestChild()
		{
			TreeNode selected = null;
			double bestValue = double.MinValue;

			foreach (var c in Children)
			{
				double uctValue = c.TotalValue/(c.NumberVisits + Epsilon) +
				                  Math.Sqrt(Math.Log(NumberVisits + 1)/(c.NumberVisits + Epsilon)) +
				                  _random.NextDouble()*Epsilon;
				if (uctValue > bestValue)
				{
					selected = c;
					bestValue = uctValue;
				}
			}

			return selected;
		}



		//simulation
		//returns 0(loss) or 1(win)
		private double GetSimulationValue(TreeNode node)
		{
			var status = node._status.Clone();

			while (!status.IsCompleted)
			{
				//TODO: DECLARATIONS???
				//if (!status.CurrentHand.IsEmpty)
				//{
					ICard[] candidates = _candidatesSelector.GetCandidates(status, status.Turn);
					int index = _random.Next(0, candidates.Length - 1);
					status = _candidatePlayer.PlayCandidate(status, status.Turn, candidates[index]);
				//}
				//else
				//{
				//    var declarations = status.Declarables;

				//    foreach (var declaration in declarations)
				//    {
				//        status.LastCompletedHand
				//    }
				//}
			}

			if (status.TeamWinner == _teamNumber)
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
	}
}