using StructureMap.Configuration.DSL;
using Subasta.Domain.Game.Analysis;
using Subasta.DomainServices;
using Subasta.Lib.Interaction;

namespace Subasta.Lib.Infrastructure
{
	internal class RegisterLibIoc : Registry
	{
		public RegisterLibIoc()
		{
			For<FrmMain>().Singleton().Use<FrmMain>();
			For<FrmGame>().Singleton().Use<FrmGame>();
			For<FrmGameInfo>().Singleton().Use<FrmGameInfo>();
			For<FrmGameSetInfo>().Singleton().Use<FrmGameSetInfo>();
			For<FrmChangeList>().Use<FrmChangeList>();


			For<ISaysExplorationListener>().Singleton().Use<NullExplorationListenerHandler>();
			For<IApplicationEventsExecutor>().Use<ApplicationEventsExecutorExecutor>();

			For<IUserInteractionManager>().Singleton().Use<UserInteractionManager>();
		}
	}
}
