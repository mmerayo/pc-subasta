using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap.Configuration.DSL;
using Subasta.DomainServices.DataAccess;
using Subasta.DomainServices.Game;
using Subasta.Infrastructure.DomainServices.DataAccess;
using Subasta.Infrastructure.DomainServices.Game;

namespace Subasta.Infrastructure.DomainServices
{
	public class DomainServicesRegistry:Registry
	{
		public DomainServicesRegistry()
		{
			For<IGameDataWritter>().Use<GameDataAllocator>();
			For<ICardComparer>().Use<CardComparer>();
			For<IGameExplorer>().Use<GameExplorer>();
			For<IGameGenerator>().Use<GameGenerator>();
			For<IPlayerDeclarationsChecker>().Use<PlayerDeclarationsChecker>();
			For<IValidCardsRule>().Use<ValidCardsRule>();
		}
	}
}
