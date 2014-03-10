using StructureMap.Configuration.DSL;

namespace Analyzer
{
	internal class RegisterAnalyzerIoc : Registry
	{
		public RegisterAnalyzerIoc()
		{
			For<FrmMain>().Singleton().Use<FrmMain>();
		}
	}
}