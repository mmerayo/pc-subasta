using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap.Configuration.DSL;
using Subasta.Domain.Deck;
using Subasta.DomainServices.DataAccess;
using Subasta.DomainServices.Game;
using Subasta.Infrastructure.Domain;
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
			For<IDeckSuffler>().Use<DeckSuffler>();
			For<ICandidatesSelector>().Use<FilteredCandidatesSelector>();
			For<IQueuedResultStoreWritter>().Use<NullStoreWritter>();
			//TODO: REMOVE THIS DEPENDENCY
			For<ISuit>().Use(Suit.Suits.First());
		}
	}
}
