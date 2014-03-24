using System;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game
{
	
	public interface ISimulator 
	{
		//NodeResult Execute(IExplorationStatus currentStatus, int playerPosition);
		//void Execute(Guid gameId, int firstPlayer, int forPlayerTeamBets, ICard[] cardsP1, ICard[] cardsP2, ICard[] cardsP3, ICard[] cardsP4, ISuit trump, int pointsBet);
		//IExplorationStatus GetInitialStatus(Guid gameId, int firstPlayer, int forPlayerTeamBets, ICard[] cardsP1, ICard[] cardsP2, ICard[] cardsP3, ICard[] cardsP4, ISuit trump, int pointsBet);
		NodeResult GetBest(IExplorationStatus currentStatus);

		int MaxDepth { get; set; }

		void Start(int teamNumber, IExplorationStatus initialStatus);
		void Stop();
		
	}
}