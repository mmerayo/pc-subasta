using System;
using System.Collections.Generic;
using System.Linq;
using Subasta.Domain;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.Domain.Game.Algorithms;

namespace Subasta.DomainServices.Game.Algorithms.MCTS
{
	internal sealed class TreeNode : IDisposable
	{
		private const double Epsilon = 1e-6;
		private static readonly Random _random = new Random((int) DateTime.UtcNow.Ticks);
		private static readonly object _typeLock = new object();

		private readonly ICandidatePlayer _candidatePlayer;
		private readonly INonFilteredCandidatesSelector _candidatesSelector;
		private readonly TreeNodeInfo[] _nodeInfos = {new TreeNodeInfo(), new TreeNodeInfo()};

		private readonly object _syncLock = new object();
		private List<TreeNode> _children;
		private Declaration? _declarationPlayed;
		private bool _disposed;
		private bool _expanded;
		private IExplorationStatus _explorationStatus;


		public TreeNode(INonFilteredCandidatesSelector candidatesSelector,
			ICandidatePlayer candidatePlayer)
		{
			_candidatesSelector = candidatesSelector;
			_candidatePlayer = candidatePlayer;
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

		public List<TreeNode> Children
		{
			get
			{
				return _children;
			}
			private set
			{
				if (_children == null)
					_children = value;
			}
		}

		public IExplorationStatus ExplorationStatus
		{
			get { return _explorationStatus; }
		}

		public ICard CardPlayed { get; private set; }

		public Declaration? DeclarationPlayed
		{
			get { return _declarationPlayed; }
			private set { _declarationPlayed = value; }
		}

		private bool IsFinal
		{
			get { return ExplorationStatus.IsCompleted; }
		}

		public void Dispose()
		{
			Dispose(true);
		}

		public void Initialize(IExplorationStatus explorationStatus)
		{
			_explorationStatus = explorationStatus.Clone();
		}

		public ITreeNodeInfo GetNodeInfo(int teamNumber)
		{
			return _nodeInfos[teamNumber - 1];
		}

		public void Select(int teamNumber)
		{
			TreeNode current = this;
			if (current.IsFinal)
				return; //end reached

			while (!current.IsLeaf)
			{
				current = current.SelectBestChild(teamNumber);
				if (current == null)
					return; //is disposing
			}

			if (current.IsFinal)
				return; //end reached

			current.Expand();
			TreeNode newNode = current.SelectBestChild(teamNumber);
			if (newNode == null)
				return; //is disposing

			SimulateAndBackPropagate(1, newNode);
			SimulateAndBackPropagate(2, newNode);
		}

		private void SimulateAndBackPropagate(byte teamNumber, TreeNode newNode)
		{
			int points;
			float simulationValue = GetSimulationValue(teamNumber, newNode, out points);
			TreeNode current = newNode;
			while (current != null)
			{
				current.UpdateStatus(teamNumber, simulationValue, points);
				current = current.Parent;
			}
		}


		private void Expand()
		{
			if (!_expanded)
				lock (_syncLock)
					if (!_expanded)
					{
						var candidates = _candidatesSelector.GetCandidates(ExplorationStatus, ExplorationStatus.Turn);
						if (candidates.Length == 0)
							throw new ApplicationException("No candidates!!");
						Children = new List<TreeNode>();
						foreach (ICard candidate in candidates)
						{
							IExplorationStatus newStatus = _candidatePlayer.PlayCandidate(ExplorationStatus, ExplorationStatus.Turn,
								candidate);

							if (newStatus.CurrentHand.IsEmpty && newStatus.Declarables.Length > 0)
							{
								Declaration[] declarables = newStatus.Declarables;

								foreach (Declaration declarable in declarables)
								{
									IExplorationStatus explorationStatus = newStatus.Clone();
									explorationStatus.LastCompletedHand.SetDeclaration(declarable);
									var treeNode = new TreeNode(_candidatesSelector, _candidatePlayer)
									{
										CardPlayed = candidate,
										_explorationStatus = explorationStatus,
										DeclarationPlayed = declarable,
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
						if (Children.Count == 0)
							throw new ApplicationException("No children!!");
						_expanded = true;
					}
		}


		public TreeNode SelectBestMove(int teamNumber)
		{
			TreeNode[] treeNodes = Children.ToArray();

			if (treeNodes.Length == 1)
				return treeNodes[0];

			int nodeInfoIdx = teamNumber - 1;
			int[] currentVisits = treeNodes.Select(x => x._nodeInfos[nodeInfoIdx].NumberVisits).ToArray();
			int repeatedCount = 0;
			while (treeNodes.Any(x => x._nodeInfos[nodeInfoIdx].NumberVisits < 10) && repeatedCount < 3)
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

			List<TreeNode> orderByDescending =
				treeNodes.OrderByDescending(x => x._nodeInfos[nodeInfoIdx].Coeficient)
					//.ThenByDescending(x => x._nodeInfos[nodeInfoIdx].AvgPoints)
					.ThenBy(x => x.ExplorationStatus.Trump == x.CardPlayed.Suit) //false first
					.ThenBy(x => x.CardPlayed.Value)
					.ThenBy(x => x.CardPlayed.Number)
					.ToList();

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
		private float GetSimulationValue(byte teamNumber, TreeNode node, out int points)
		{
			IExplorationStatus currentStatus = node.ExplorationStatus.Clone();

			while (!currentStatus.IsCompleted)
			{
				ICard[] candidates = _candidatesSelector.GetCandidates(currentStatus, currentStatus.Turn);
				int index = _random.Next(0, candidates.Length - 1);
				currentStatus = _candidatePlayer.PlayCandidate(currentStatus, currentStatus.Turn, candidates[index]);

				//Add the most valuable declaration
				if (currentStatus.CurrentHand.IsEmpty)
				{
					Declaration[] declarations = currentStatus.Declarables;
					if (declarations != null && declarations.Length > 0)
					{
						Declaration declaration = declarations.OrderByDescending(DeclarationValues.ValueOf).First();
						currentStatus.LastCompletedHand.SetDeclaration(declaration);
					}
				}
			}
			int result = currentStatus.TeamWinner == teamNumber ? 1 : 0;
			points = currentStatus.SumTotalTeam(teamNumber);
			return result;
		}

		private void UpdateStatus(int teamNumber, float value, int points)
		{
			lock (_syncLock)
			{
				TreeNodeInfo treeNodeInfo = _nodeInfos[teamNumber - 1];
				treeNodeInfo.RecordExploration(points, value);
			}
		}

		public override string ToString()
		{
			return string.Format("{0} - {1}", CardPlayed, DeclarationPlayed);
		}


		private void Dispose(bool disposing)
		{
			lock (_syncLock)
			{
				Parent = null;
				if (Children != null)
				{
					Children.ForEach(x => x.Dispose());
					Children.Clear();
				}
				_disposed = true;
			}
		}

		~TreeNode()
		{
			Dispose(false);
		}

		private class TreeNodeInfo : ITreeNodeInfo
		{
			private readonly Dictionary<int, int> _ocurrences = new Dictionary<int, int>();

			public float Coeficient
			{
				get { return TotalValue/NumberVisits; }
			}

			public float TotalValue { get; private set; }
			public float AvgPoints { get; private set; }
			public int NumberVisits { get; private set; }

			public float PercentageChancesOfMaking(int points)
			{
				lock (this)
				{
					int sum = _ocurrences.Keys.Where(x => x >= points).Sum(key => _ocurrences[key]);

					return (sum/(float) NumberVisits);
				}
			}

			public int GetMaxPointsWithMinimumChances(double percentaje)
			{
				lock (this)
				{
					var candidates = new List<int>(25);
					for (int i = 0; i < 25; i++)
					{
						if (PercentageChancesOfMaking(i) > percentaje)
							candidates.Add(i);
					}
					return candidates.Max()*10;
				}
			}

			public void RecordExploration(int points, float value)
			{
				lock (this)
				{
					NumberVisits++;
					TotalValue += value;
					AvgPoints = (AvgPoints + points)/2;

					var normalizedPoints = (int) Math.Truncate((double) (points/10));
					if (!_ocurrences.ContainsKey(normalizedPoints))
						_ocurrences.Add(normalizedPoints, 0);

					_ocurrences[normalizedPoints] = _ocurrences[normalizedPoints] + 1;
				}
			}
		}
	}
}