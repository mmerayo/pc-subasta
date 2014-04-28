using StructureMap.Configuration.DSL;
using Subasta.Domain.Game.Analysis;
using Subasta.DomainServices;
using Subasta.Lib.Interaction;

namespace Subasta.Lib.Infrastructure
{
	internal class RegisterIoc : Registry
	{
		public RegisterIoc()
		{
			For<FrmMain>().Singleton().Use<FrmMain>();
			For<FrmGame>().Singleton().Use<FrmGame>();
			For<FrmGameInfo>().Singleton().Use<FrmGameInfo>();
			For<FrmSays>().Singleton().Use<FrmSays>();
			For<FrmGameSetInfo>().Singleton().Use<FrmGameSetInfo>();

			For<ISaysExplorationListener>().Singleton().Use<NullExplorationListenerHandler>();
			For<IApplicationEventsExecutor>().Use<ApplicationEventsExecutorExecutor>();

			For<IUserInteractionManager>().Singleton().Use<UserInteractionManager>();
		}
	}
}
