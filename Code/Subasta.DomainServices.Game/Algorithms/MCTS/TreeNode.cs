using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Subasta.Domain;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.MCTS
{
	
	internal sealed class TreeNode
	{
		private const double Epsilon = 1e-6;
		private readonly INonFilteredCandidatesSelector _candidatesSelector;
		private readonly IExplorationStatus _status;
		private readonly ICandidatePlayer _candidatePlayer;
		static readonly Random _random=new Random((int)DateTime.UtcNow.Ticks);

		public static TreeNode RootTeam1
		{
			get;
			private set;
		}

		public static TreeNode RootTeam2
		{
			get;
			private set;
		}

		public static void Initialize(INonFilteredCandidatesSelector candidatesSelector,
			IExplorationStatus status, ICandidatePlayer candidatePlayer)
		{
			RootTeam1 = new TreeNode(candidatesSelector,status,candidatePlayer);
			RootTeam2 = new TreeNode(candidatesSelector, status, candidatePlayer);
		}


		private TreeNode(INonFilteredCandidatesSelector candidatesSelector, 
			IExplorationStatus status, 
			ICandidatePlayer candidatePlayer)
		{
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
						Children.Add(new TreeNode(_candidatesSelector, newStatus, _candidatePlayer));
					}
				}
				else
				{
					Children.Add(new TreeNode(_candidatesSelector, newStatus, _candidatePlayer));
				}
			}
		}

		private TreeNode SelectBestChild()
		{
			TreeNode selected = null;
			double bestValue = double.MinValue;

			foreach (var c in Children)
			{
				double uctValue = c.TotalValue / (c.NumberVisits + Epsilon) +
					   Math.Sqrt(Math.Log(NumberVisits + 1) / (c.NumberVisits + Epsilon)) +
						   _random.NextDouble() * Epsilon;
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
			//if is end of path performs the calculation

			//it creates the child nodes as _not expanded if they were not created yet
			
			//selects one and keep going

			throw new NotImplementedException();
		}

		private void UpdateStatus(double value)
		{
			NumberVisits++;
			TotalValue += value;
		}

		
	}
}
