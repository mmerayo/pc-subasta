using System;
using System.Collections.Generic;
using AutoMapper;
using NHibernate;
using Subasta.Domain.Deck;
using Subasta.DomainServices.DataAccess.Sqlite.Models;

namespace Subasta.DomainServices.DataAccess.Sqlite.Writters
{
    class GameSettingsWritter:IGameSettingsStoreWritter
    {
        private readonly IGameDataHelper _dataHelper;

        public GameSettingsWritter(IGameDataHelper dataHelper)
        {
            _dataHelper = dataHelper;
        }

        public void StoreGameInfo(Guid gameId, int firstPlayer, ISuit trump, ICard[] cardsP1, ICard[] cardsP2, ICard[] cardsP3, ICard[] cardsP4)
        {
            var players = new List<PlayerGameInfo>
                              {
                                  new PlayerGameInfo(1, Mapper.Map<CardInfo[]>(cardsP1)),
                                  new PlayerGameInfo(2, Mapper.Map<CardInfo[]>(cardsP2)),
                                  new PlayerGameInfo(3,Mapper.Map<CardInfo[]>(cardsP3)),
                                  new PlayerGameInfo(4,Mapper.Map<CardInfo[]>(cardsP4))
                              };

            var gameInfo = new GameInfo
                               {
                                   FirstPlayer = firstPlayer,
                                   TrumpSuit = trump.Name,
                                   Players = players
                               };
           
               using (var uow= _dataHelper.GetUnitOfWork<ISession>(gameId))
               {
                   uow.Session.Save(gameInfo);
               }

        }
    }
}