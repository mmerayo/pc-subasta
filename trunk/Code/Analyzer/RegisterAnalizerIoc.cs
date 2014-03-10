using StructureMap.Configuration.DSL;

namespace Analyzer
{
	internal class RegisterAnalizerIoc : Registry
	{
		public RegisterAnalizerIoc()
		{
			For<FrmMain>().Singleton().Use<FrmMain>();
		}
	}
}