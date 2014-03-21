using StructureMap.Configuration.DSL;
using Subasta.ApplicationServices;

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