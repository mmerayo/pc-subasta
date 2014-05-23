using StructureMap.Configuration.DSL;
using Subasta.Domain.Game;
using Subasta.DomainServices.Game.Algorithms.Figures;
using Subasta.DomainServices.Game.Algorithms.Figures.Catalog;
using Subasta.DomainServices.Game.Algorithms.MCTS;
using Subasta.DomainServices.Game.Algorithms.MCTS.Diagnostics;
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
			For<IPlayerDeclarationsChecker>().Use<PlayerDeclarationsChecker>();
			For<IValidCardsRule>().Use<ValidCardsRule>();
			For<IDeckShuffler>().Singleton().Use<DeckShuffler>();
			For<ICandidatesSelector>().Use<FilteredCandidatesSelector>();
			For<INonFilteredCandidatesSelector>().Use<NonFilteredCandidatesSelector>();
			For<ICandidatePlayer>().Use<CandidatePlayer>();


			For<IMctsRunner>().Singleton().Use<MctsRunner>();
			For<IMctsSaysRunner>().Singleton().Use<MctsSaysRunner>();
			For<ISimulator>().Use(x => x.GetInstance<IMctsRunner>());
			For<IMctsDiagnostics>().Use<MctsDiagnostics>();


			For<IFiguresSolver>().Use<FiguresSolver>();
			For<IFigure>().Use<FigurePaso>();
			For<IFigure>().Use<FigureAs>();
			For<IFigure>().Use<FigureParejaSegura>();
			For<IFigure>().Use<FigureParejaConAs>();
			For<IFigure>().Use<FigureNada>();
			For<IFigure>().Use<FigureParejaNoSegura>();
			For<IFigure>().Use<FigureCaballos>();
			For<IFigure>().Use<FigureTreses>();
			For<IFigure>().Use<FigureParejaNoSeguraSinDominarPalo>();
			For<IFigure>().Use<FigurePaloCorrido>();
			For<IFigure>().Use<FigureReyes>();
			For<IFiguresCatalog>().Use<FiguresCatalog>();
			For<ISaysSimulator>().Use(x => x.GetInstance<IMctsSaysRunner>());

			For<IGame>().Use<Players.Game>();
			For<IMctsPlayer>().Use<MctsPlayer>();
			For<IHumanPlayer>().Use<HumanPlayer>();

		}
	}
}
