using System;
using Subasta.Domain.Deck;

namespace Subasta.DomainServices.DataAccess.Sqlite.Writters
{
	internal class NullGameSettingsWritter:IGameSettingsStoreWritter
	{
		public void StoreGameInfo(Guid gameId, int firstPlayer, int teamBet, ISuit trump, ICard[] cardsP1, ICard[] cardsP2, ICard[] cardsP3, ICard[] cardsP4)
		{
		}
	}
}