﻿using System;
using System.Linq;
using Subasta.Domain;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.DomainServices.DataAccess;

namespace Subasta.DomainServices.Game.Algorithms.MaxN
{
	internal class MaxNSimulator : IMaxNSimulator
	{
		private readonly ICandidatesSelector _candidatesSelector;
		private readonly IResultStoreWritter _resultsWritter;
		private readonly ICandidatePlayer _candidatePlayer;
		private int _maxDepth;
		

		public MaxNSimulator(ICandidatesSelector candidatesSelector,
		                    IQueuedResultStoreWritter resultsWritter,
		                   
			ICandidatePlayer candidatePlayer)
		{
			
			if (resultsWritter == null) throw new ArgumentNullException("resultsWritter");
			_candidatesSelector = candidatesSelector;
			_resultsWritter = resultsWritter;
			_candidatePlayer = candidatePlayer;
		}

		//public void Execute(Guid gameId, int firstPlayer, int forPlayerTeamBets, ICard[] cardsP1, ICard[] cardsP2,
		//                    ICard[] cardsP3,
		//                    ICard[] cardsP4,
		//                    ISuit trump, int pointsBet)
		//{
		//    try
		//    {
		//        var status = GetInitialStatus(gameId, firstPlayer, forPlayerTeamBets, cardsP1, cardsP2, cardsP3, cardsP4, trump,
		//                                      pointsBet);
		//        Execute(status, firstPlayer);
		//    }
		//    catch (Exception ex)
		//    {
		//        Console.WriteLine(ex);
		//        throw;
		//    }
		//}


		public NodeResult GetBest(IExplorationStatus currentStatus)
		{
			NodeResult nodeResult = Execute(currentStatus, currentStatus.Turn);
			return nodeResult;
		}

		public int MaxDepth
		{
			get { return _maxDepth; }
			set
			{
				if(value<0) throw new ArgumentOutOfRangeException();
				_maxDepth = value;
			}
		}

		public IPlayer Player { get; set; }

		public void Start(int teamNumber, IExplorationStatus initialStatus)
		{
			
		}

		public void Stop()
		{

		}

		private NodeResult Execute(IExplorationStatus currentStatus, int playerPlays)
		{
			if (IsTerminalNode(currentStatus, playerPlays))
			{
				//Console.WriteLine("***Terminal Node");
				var nodeResult = new NodeResult(currentStatus);
				StoreResult(nodeResult);

				return nodeResult;
			}

			var candidates =_candidatesSelector. GetCandidates(currentStatus, playerPlays);

			IExplorationStatus updatedStatus;
			var best = Explore(currentStatus, playerPlays, candidates[0], out updatedStatus);
			var length = candidates.Length;
			for (int i = 1; i < length; i++)
			{
				ICard candidate = candidates[i];
				var current = Explore(currentStatus, playerPlays, candidate, out updatedStatus);

				int currentPoints = current[playerPlays];
				int currentBestPoints = best[playerPlays];
				if (currentPoints > currentBestPoints || (currentPoints == currentBestPoints && candidate.IsAbsSmallerThan(best.CardAtMove(playerPlays,currentStatus.CurrentHand.Sequence) )))
				{
					//Console.WriteLine("---Found better move");
					best = current;
				}
			}

			return best;
		}

		private void StoreResult(NodeResult result)
		{
			_resultsWritter.Add(result);
		}

		//explora los cantes tambien
		private NodeResult Explore(IExplorationStatus currentStatus, int playerPosition, ICard candidate,
		                           out IExplorationStatus updatedStatus)
		{
			updatedStatus = _candidatePlayer.PlayCandidate(currentStatus, playerPosition, candidate);

			var declarables = updatedStatus.Declarables;
			var newStatus = updatedStatus.Clone();
			if (declarables.Length > 0)
				AddDeclaration(declarables[0], newStatus);

			var best = Execute(newStatus, newStatus.Turn);

			int length = declarables.Length;
			for (int index = 1; index < length; index++)
			{
				var declaration = declarables[index];
				newStatus = updatedStatus.Clone();
				AddDeclaration(declaration, newStatus);

				var current = Execute(newStatus, newStatus.Turn);

				if (current[playerPosition] > best[playerPosition])
					best = current;

			}

			return best;
		}

		private static void AddDeclaration(Declaration declaration, IExplorationStatus newStatus)
		{
			IHand firstDeclarableHand = newStatus.FirstDeclarableHand;
			if (firstDeclarableHand != null && newStatus.IsInTeamBets(firstDeclarableHand.PlayerWinner.Value))
				firstDeclarableHand.SetDeclaration(declaration);
		}


		private bool IsTerminalNode(IExplorationStatus currentStatus, int playerPosition)
		{
		    if (currentStatus.Hands.Count(x => x.IsCompleted) == MaxDepth) return true;

			return currentStatus.IsCompleted;

		    //return currentStatus.PlayerCards(playerPosition).Length == 0;
		}
	    
	}
}