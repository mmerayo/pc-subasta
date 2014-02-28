using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using AutoMapper;
using NHibernate;
using Subasta.Domain.Game;
using Subasta.DomainServices.DataAccess.Sqlite.Models;

namespace Subasta.DomainServices.DataAccess.Sqlite.Writters
{
    //TODO:Decorate with a queue async batch processor
	internal class ResultStoreWritter : IResultStoreWritter
	{
		private readonly IGameDataHelper _dataHelper;

		public ResultStoreWritter(IGameDataHelper dataHelper)
		{
			_dataHelper = dataHelper;
		}

		public void Add(NodeResult result)
		{
			var explorationInfo = Mapper.Map<ExplorationInfo>(result);
			using (var uow = _dataHelper.GetUnitOfWork<IStatelessSession>(result.GameId))
			{
				Add(explorationInfo, uow);
				uow.Commit();
			}
		}

		private void Add(ExplorationInfo entity, IUnitOfWork<IStatelessSession> unitOfWork)
		{
			//DENORMALIZE GAMEINFO ALTOGHETEHER WITH EXPLORATION INFO
			foreach (var handInfo in entity.Hands)
			{
				handInfo.Cards = StaticDataReader.GetDbCards(unitOfWork.Session, handInfo.Cards);
				unitOfWork.Session.Insert(handInfo);
			}

			entity.Game = unitOfWork.Session.QueryOver<GameInfo>().Where(x => x.TrumpSuit == entity.Trump && x.TeamBet==entity.TeamBet).SingleOrDefault();

			unitOfWork.Session.Insert(entity);
		}


		public void Add(IEnumerable<NodeResult> results)
		{
			if (!results.Any())
				return;
			IEnumerable<Guid> gameIds = results.Select(x => x.GameId).Distinct();
			Debug.Assert(gameIds.Count() == 1);
			Guid gameId = gameIds.ElementAt(0);
			var explorationInfos = Mapper.Map<IEnumerable<ExplorationInfo>>(results);
			using (var uow = _dataHelper.GetUnitOfWork<IStatelessSession>(gameId))
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
