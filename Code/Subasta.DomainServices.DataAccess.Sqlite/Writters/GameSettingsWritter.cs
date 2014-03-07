using System;
using System.Collections.Generic;
using System.Data;
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

		public void StoreGameInfo(Guid gameId, int firstPlayer,int teamBet, ISuit trump, ICard[] cardsP1, ICard[] cardsP2, ICard[] cardsP3,
		                          ICard[] cardsP4)
		{

			using (var uow = _dataHelper.GetUnitOfWork<ISession>(gameId))
			{
				var players = new List<PlayerGameInfo>
				              	{
				              		new PlayerGameInfo(1, StaticDataReader.GetDbCards(uow.Session, cardsP1)),
				              		new PlayerGameInfo(2, StaticDataReader.GetDbCards(uow.Session, cardsP2)),
				              		new PlayerGameInfo(3,StaticDataReader. GetDbCards(uow.Session, cardsP3)),
				              		new PlayerGameInfo(4,StaticDataReader. GetDbCards(uow.Session, cardsP4))
				              	};

				var gameInfo = new GameInfo
				               	{
				               		FirstPlayer = firstPlayer,
				               		TrumpSuit = trump.Name,
				               		Players = players,
									TeamBet = teamBet
				               	};

				players.ForEach(x => uow.Session.Save(x));

				uow.Session.Save(gameInfo);
				uow.Commit();
			}
		}

		
	}
}