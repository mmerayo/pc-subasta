using StructureMap.Configuration.DSL;
using Subasta.ApplicationServices;
using Subasta.Client.Common.Infrastructure;
using Subasta.Domain.Game.Analysis;
using Subasta.DomainServices;

namespace Analyzer
{
	internal class RegisterAnalyzerIoc : Registry
	{
		public RegisterAnalyzerIoc()
		{
			For<FrmMain>().Singleton().Use<FrmMain>();
			For<FrmExplorationStatus>().Singleton().Use<FrmExplorationStatus>();
			For<FrmGameExplorationAnalyzer>().Use<FrmGameExplorationAnalyzer>();

			For<ISaysExplorationListener>().Singleton().Use<ExplorationListenerHandler>();
			For<IApplicationEventsExecutor>().Use<ApplicationEventsExecutorExecutor>();
			For<IViewLoader>().Use<ViewLoader>();
		}
	}
}