using StructureMap.Configuration.DSL;
using Subasta.ApplicationServices;
using Subasta.DomainServices;

namespace Analyzer
{
	internal class RegisterAnalyzerIoc : Registry
	{
		public RegisterAnalyzerIoc()
		{
			For<FrmMain>().Singleton().Use<FrmMain>();
			For<FrmExplorationStatus>().Singleton().Use<FrmExplorationStatus>();
			For<IApplicationEventsExecutor>().Use<ApplicationEventsExecutorExecutor>();
		}
	}
}