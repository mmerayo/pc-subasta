using System;
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

        public void StoreGameInfo(int firstPlayer, ISuit trump, ICard[] cardsP1, ICard[] cardsP2, ICard[] cardsP3, ICard[] cardsP4)
        {
            var gameInfo = new GameInfo
                               {
                                   FirstPlayer = firstPlayer,
                                   TrumpSuit = trump.Name,
                                   CardsP1 = Mapper.Map<CardInfo[]>(cardsP1),
                                   CardsP2 = Mapper.Map<CardInfo[]>(cardsP2),
                                   CardsP3 = Mapper.Map<CardInfo[]>(cardsP3),
                                   CardsP4 = Mapper.Map<CardInfo[]>(cardsP4)
                               };
           
               using (var uow= _dataHelper.GetUnitOfWork<ISession>())
               {
                   uow.Session.Save(gameInfo);
               }

        }
    }
}