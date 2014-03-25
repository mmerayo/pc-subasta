using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap.Configuration.DSL;
using Subasta.Domain.Deck;
using Subasta.DomainServices.DataAccess;
using Subasta.DomainServices.Factories;
using Subasta.DomainServices.Game;
using Subasta.Infrastructure.Domain;
using Subasta.Infrastructure.DomainServices.DataAccess;
using Subasta.Infrastructure.DomainServices.Factories;


namespace Subasta.Infrastructure.DomainServices
{
	public class DomainServicesRegistry:Registry
	{
		public DomainServicesRegistry()
		{
			For<IGameDataWritter>().Use<GameDataAllocator>();
			
			For<IQueuedResultStoreWritter>().Use<NullStoreWritter>();
			//TODO: REMOVE THIS DEPENDENCY
			//For<ISuit>().Use(Suit.Suits.First());
			For<IPlayerFactory>().Use<PlayerFactory>();
		}
	}
}
