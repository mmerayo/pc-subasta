using StructureMap.Configuration.DSL;
using Subasta.Domain.Game;
using Subasta.DomainServices.Game.Algorithms.MCTS;
using Subasta.DomainServices.Game.Players;
using Subasta.DomainServices.Game.Strategies;
using Subasta.DomainServices.Game.Utils;

namespace Subasta.DomainServices.Game
{
	public class GameRegistry:Registry
	{
		public GameRegistry()
		{
			For<ICardComparer>().Use<CardComparer>();
			For<IGameGenerator>().Use<GameGenerator>();
			For<IPlayerDeclarationsChecker>().Use<PlayerDeclarationsChecker>();
			For<IValidCardsRule>().Use<ValidCardsRule>();
			For<IDeckSuffler>().Use<DeckSuffler>();
			For<ICandidatesSelector>().Use<FilteredCandidatesSelector>();
			For<INonFilteredCandidatesSelector>().Use<NonFilteredCandidatesSelector>();
			For<ICandidatePlayer>().Use<CandidatePlayer>();


			For<IMctsRunner>().Singleton().Use<MctsRunner>();
			For<IMctsSaysRunner>().Singleton().Use<MctsSaysRunner>();

			For<ISimulator>().Use(x => x.GetInstance<IMctsRunner>());
			For<ISaysSimulator>().Use(x => x.GetInstance<IMctsSaysRunner>());

			For<IGame>().Use<Players.Game>();
			For<IMctsPlayer>().Use<MctsPlayer>();
			For<IHumanPlayer>().Use<HumanPlayer>();
		}
	}
}
