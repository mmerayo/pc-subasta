using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap.Configuration.DSL;
using Subasta.DomainServices.Game.Algorithms;
using Subasta.DomainServices.Game.Algorithms.MCTS;
using Subasta.DomainServices.Game.Algorithms.MaxN;
using Subasta.DomainServices.Game.Strategies;
using Subasta.DomainServices.Game.Utils;

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
			For<ICandidatePlayer>().Use<CandidatePlayer>();


			For<IMctsRunner>().Use<MctsRunner>();
			For<IMctsTreeCommands>().Use<MctsTreeCommands>();
		}
	}
}
