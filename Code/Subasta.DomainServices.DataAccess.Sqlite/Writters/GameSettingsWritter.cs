using System;
using System.Collections.Generic;
using AutoMapper;
using NHibernate;
using Subasta.Domain.Deck;
using Subasta.DomainServices.DataAccess.Sqlite.Models;

namespace Subasta.DomainServices.DataAccess.Sqlite.Writters
{
	internal class GameSettingsWritter : IGameSettingsStoreWritter
	{
		private readonly IGameDataHelper _dataHelper;

		public GameSettingsWritter(IGameDataHelper dataHelper)
		{
			_dataHelper = dataHelper;
		}

		public void StoreGameInfo(Guid gameId, int firstPlayer, ISuit trump, ICard[] cardsP1, ICard[] cardsP2, ICard[] cardsP3,
		                          ICard[] cardsP4)
		{

			using (var uow = _dataHelper.GetUnitOfWork<ISession>(gameId))
			{
				var players = new List<PlayerGameInfo>
				              	{
				              		new PlayerGameInfo(1, GetDbCards(uow.Session, cardsP1)),
				              		new PlayerGameInfo(2, GetDbCards(uow.Session, cardsP2)),
				              		new PlayerGameInfo(3, GetDbCards(uow.Session, cardsP3)),
				              		new PlayerGameInfo(4, GetDbCards(uow.Session, cardsP4))
				              	};

				var gameInfo = new GameInfo
				               	{
				               		FirstPlayer = firstPlayer,
				               		TrumpSuit = trump.Name,
				               		Players = players
				               	};

				players.ForEach(x => uow.Session.Save(x));

				uow.Session.Save(gameInfo);
				uow.Commit();
			}
		}

		private IEnumerable<CardInfo> GetDbCards(ISession session, IEnumerable<ICard> cards)
		{
			var result = new List<CardInfo>();
			foreach (var card in cards)
			{
				var dbItem =
					session.QueryOver<CardInfo>().Where(x => x.Suit == card.Suit.Name).And(y => y.Number == card.Number).
						SingleOrDefault();
				
				result.Add(dbItem);
			}

			return result;
		}
	}
}