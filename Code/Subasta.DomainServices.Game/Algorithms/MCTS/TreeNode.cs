using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using Subasta.Domain;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.MCTS
{
	internal interface ITreeNodeInfo
	{
		double Coeficient { get; }

		double TotalValue { get; }
		double AvgPoints { get; }
		int NumberVisits { get; }
	}

	internal sealed class TreeNode : IDisposable
	{
		private class TreeNodeInfo : ITreeNodeInfo
		{
			public double Coeficient
			{
				get { return TotalValue/NumberVisits; }
			}

			public double TotalValue { get; set; }
			public double AvgPoints { get; set; }
			public int NumberVisits { get; set; }
		}

		private static readonly Random _random = new Random((int) DateTime.UtcNow.Ticks);

		private const double Epsilon = 1e-6;

		private readonly INonFilteredCandidatesSelector _candidatesSelector;
		private IExplorationStatus _explorationStatus;
		private readonly ICandidatePlayer _candidatePlayer;

		private readonly object _syncLock = new object();
		private ICard _cardPlayed;
		private Declaration? _declarationPlayed;
		private static readonly object _typeLock = new object();
		private bool _expanded;
		private List<TreeNode> _children;


		public TreeNode(INonFilteredCandidatesSelector candidatesSelector,
		ICandidatePlayer candidatePlayer)
		{
		
			_candidatesSelector = candidatesSelector;
			_candidatePlayer = candidatePlayer;
		}

		public void Initialize(IExplorationStatus explorationStatus)
		{
			_explorationStatus = explorationStatus.Clone();
		}


		public bool IsLeaf
		{
			get
			{
				lock (_syncLock)
					return !_expanded;
			}
		}

		private TreeNode Parent { get; set; }

		private readonly TreeNodeInfo[] _nodeInfos = new TreeNodeInfo[] {new TreeNodeInfo(), new TreeNodeInfo()};

		public ITreeNodeInfo GetNodeInfo(int teamNumber)
		{
			return _nodeInfos[teamNumber - 1];
		}

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
			get { return _explorationStatus; }
		}

		public ICard CardPlayed
		{
			get { return _cardPlayed; }
			private set { _cardPlayed = value; }
		}

		public Declaration? DeclarationPlayed
		{
			get { return _declarationPlayed; }
			private set { _declarationPlayed = value; }
		}

		public void Select(int teamNumber)
		{
			var current = this;
			if (current.IsFinal)
				return; //end reached

			while (!current.IsLeaf)
			{
				current = current.SelectBestChild(teamNumber);
				if(current==null)
					return;//is disposing
			}

			if (current.IsFinal)
				return; //end reached

			current.Expand();
			var newNode = current.SelectBestChild(teamNumber);
			if (newNode == null)
				return;//is disposing

			SimulateAndBackPropagate(1, newNode);
			SimulateAndBackPropagate(2, newNode);
		}
			
		private void SimulateAndBackPropagate(int teamNumber, TreeNode newNode)
		{
			int points;
			var simulationValue = GetSimulationValue(teamNumber, newNode, out points);
			var current = newNode;
			while (current != null)
			{
				current.UpdateStatus(teamNumber, simulationValue, points);
				current = current.Parent;
			}
		}


		private bool IsFinal
		{
			get { return ExplorationStatus.IsCompleted; }
		}

		private void Expand()
		{
			if (!_expanded)
				lock (_syncLock)
					if (!_expanded)
					{
						ICard[] candidates = _candidatesSelector.GetCandidates(ExplorationStatus, ExplorationStatus.Turn);
						if (candidates.Length == 0)
							throw new ApplicationException("No candidates!!");
						Children = new List<TreeNode>();
						foreach (var candidate in candidates)
						{
							var newStatus = _candidatePlayer.PlayCandidate(ExplorationStatus, ExplorationStatus.Turn,
								candidate);

							if (newStatus.CurrentHand.IsEmpty && newStatus.Declarables.Length > 0)
							{
								var declarations = newStatus.Declarables;

								foreach (var declaration in declarations)
								{
									newStatus.LastCompletedHand.SetDeclaration(declaration);
									var treeNode = new TreeNode(_candidatesSelector, _candidatePlayer)
									{
										CardPlayed = candidate,
										_explorationStatus = newStatus,
										DeclarationPlayed = declaration,
										Parent = this

									};
									Children.Add(treeNode);
								}
							}
							else
							{
								Children.Add(new TreeNode(_candidatesSelector, _candidatePlayer)
								{
									CardPlayed = candidate,
									_explorationStatus = newStatus,
									Parent = this
								});
							}
						}
						if (Children.Count== 0)
							throw new ApplicationException("No children!!");
						_expanded = true;
					}
		}


			
		public TreeNode SelectBestMove(int teamNumber)
		{
			var treeNodes = Children.ToArray();

			if (treeNodes.Length == 1)
				return treeNodes[0];

			int nodeInfoIdx = teamNumber - 1;

			int[] currentVisits = treeNodes.Select(x => x._nodeInfos[nodeInfoIdx].NumberVisits).ToArray();
			int repeatedCount = 0;
			while (treeNodes.Any(x => x._nodeInfos[nodeInfoIdx].NumberVisits < 10) && repeatedCount<3)
			{
				//Debug.WriteLine(string.Join("-", treeNodes.Select(x => x._nodeInfos[nodeInfoIdx].NumberVisits)));
				Select(teamNumber);

				int[] visits = treeNodes.Select(x => x._nodeInfos[nodeInfoIdx].NumberVisits).ToArray();
				if (visits.SequenceEqual(currentVisits))
				{
					repeatedCount++;
				}
				else
				{
					currentVisits = visits;
					repeatedCount = 0;
				}
			}

			var orderByDescending = treeNodes.OrderByDescending(x => x._nodeInfos[nodeInfoIdx].Coeficient).ToList();
			const double tolerance = 0.0001;
			if (orderByDescending.Count > 1 &&
				(Math.Abs(orderByDescending[0]._nodeInfos[nodeInfoIdx].Coeficient -
						  orderByDescending[1]._nodeInfos[nodeInfoIdx].Coeficient) < tolerance))
			{
				var a = new[] {orderByDescending[0], orderByDescending[1]};
				return a.OrderByDescending(x => x._nodeInfos[nodeInfoIdx].AvgPoints).First();
			}

			return orderByDescending.First();
		   
		}

		private TreeNode SelectBestChild(int teamNumber)
		{
			if (_disposed)
			{
				return null;
			}
			return Children.OrderBy(x => x._nodeInfos[teamNumber - 1].NumberVisits).FirstOrDefault();

		}

		//simulation
		//returns 0(loss) or 1(win)
		private double GetSimulationValue(int teamNumber, TreeNode node, out int points)
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
			var result = currentStatus.TeamWinner == teamNumber ? 1 : 0;
			points = currentStatus.SumTotalTeam(teamNumber);
			return result;
		}

		private void UpdateStatus(int teamNumber, double value, int points)
		{
			lock (_syncLock)
			{
				TreeNodeInfo treeNodeInfo = _nodeInfos[teamNumber - 1];
				treeNodeInfo.NumberVisits++;
				treeNodeInfo.TotalValue += value;
				treeNodeInfo.AvgPoints = (treeNodeInfo.AvgPoints + points)/2;
			}
		}


		private bool _disposed = false;

		public void Dispose()
		{
			Dispose(true);
		}

		private void Dispose(bool disposing)
		{
			Parent = null;
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