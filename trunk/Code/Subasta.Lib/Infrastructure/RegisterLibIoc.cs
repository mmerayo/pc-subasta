using StructureMap.Configuration.DSL;
using Subasta.Client.Common.Game;
using Subasta.Client.Common.Media;
using Subasta.Domain.Game.Analysis;
using Subasta.DomainServices;
using Subasta.Lib.Interaction;
using Subasta.Lib.Media;

namespace Subasta.Lib.Infrastructure
{
	internal class RegisterLibIoc : Registry
	{
		public RegisterLibIoc()
		{
			For<FrmMain>().Singleton().Use<FrmMain>();
			For<FrmGameScreen>().Singleton().Use<FrmGameScreen>();
			For<FrmChangeList>().Use<FrmChangeList>();


			For<ISaysExplorationListener>().Singleton().Use<NullExplorationListenerHandler>();
			For<IApplicationEventsExecutor>().Use<ApplicationEventsExecutorExecutor>();

			For<IUserInteractionManager>().Singleton().Use<UserInteractionManager>();
			For<ISoundPlayer>().Singleton().Use<SoundPlayer>();
		}
	}
}
