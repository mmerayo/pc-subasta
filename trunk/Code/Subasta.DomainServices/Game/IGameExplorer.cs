using System;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game
{
	public interface IGameExplorer
	{
		//NodeResult Execute(IExplorationStatus currentStatus, int playerPosition);
		void Execute(Guid gameId, int firstPlayer, ICard[] cardsP1, ICard[] cardsP2, ICard[] cardsP3, ICard[] cardsP4, ISuit trump);
	}
}