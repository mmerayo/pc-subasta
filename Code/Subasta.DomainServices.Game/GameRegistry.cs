using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap.Configuration.DSL;

namespace Subasta.DomainServices.Game
{
	public class GameRegistry:Registry
	{
		public GameRegistry()
		{
			For<ICardComparer>().Use<CardComparer>();
			For<ISimulator>().Use<MaxNSimulator>();
			For<IGameGenerator>().Use<GameGenerator>();
			For<IPlayerDeclarationsChecker>().Use<PlayerDeclarationsChecker>();
			For<IValidCardsRule>().Use<ValidCardsRule>();
			For<IDeckSuffler>().Use<DeckSuffler>();
			For<ICandidatesSelector>().Use<FilteredCandidatesSelector>();
		}
	}
}
