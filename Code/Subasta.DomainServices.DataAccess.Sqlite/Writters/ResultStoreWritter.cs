using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AutoMapper;
using NHibernate;
using Subasta.Domain.Game;
using Subasta.DomainServices.DataAccess.Sqlite.Models;

namespace Subasta.DomainServices.DataAccess.Sqlite.Writters
{
    //TODO:Decorate with a queue async batch processor
    class ResultStoreWritter:IResultStoreWritter
    {
        private readonly IGameDataHelper _dataHelper;

        public ResultStoreWritter(IGameDataHelper dataHelper)
        {
            _dataHelper = dataHelper;
        }

        public void Add(NodeResult result)
        {
            var explorationInfo = Mapper.Map<ExplorationInfo>(result);
            using(var uow=_dataHelper.GetUnitOfWork<ISession>(result.GameId))
            {
                uow.Session.Save(explorationInfo);
                uow.Commit();
            }
        }

        private void Add(ExplorationInfo entity,IUnitOfWork<ISession> unitOfWork)
        {
            unitOfWork.Session.Save(entity);
        }


        public void Add(IEnumerable<NodeResult> results)
        {
            if (results.Count() == 0)
                return;
            IEnumerable<Guid> gameIds = results.Select(x => x.GameId).Distinct();
            Debug.Assert(gameIds.Count() == 1);
            Guid gameId = gameIds.ElementAt(0);
            var explorationInfos = Mapper.Map<IEnumerable<ExplorationInfo>>(results);
            using (var uow = _dataHelper.GetUnitOfWork<ISession>(gameId))
            {
                foreach (var explorationInfo in explorationInfos)
                {
                    Add(explorationInfo, uow);
                }
                uow.Commit();
            }
        }
    }
}
