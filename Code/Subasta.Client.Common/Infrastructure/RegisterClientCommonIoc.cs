﻿using StructureMap.Configuration.DSL;
using Subasta.Client.Common.Game;
using Subasta.Client.Common.Media;
using Subasta.Client.Common.Storage;
using Subasta.DomainServices.Dal;

namespace Subasta.Client.Common.Infrastructure
{
	public class RegisterClientCommonIoc : Registry
	{
		public RegisterClientCommonIoc()
		{
			For<IGameHandler>().Use<GameHandler>();
#if DEBUG
			For<IGameSetHandler>().Singleton().Use<GameSetHandlerDebug>();
#else
			For<IGameSetHandler>().Singleton().Use<GameSetHandler>();
#endif
			For<IStoredGameReader>().Use<StoredGameReader>();
			For<IStoredGameWritter>().Use<StoredGameWritter>();
			For<IStoredGamesCommands>().Use<StoredGamesCommands>();
			For<IResourceReadingUtils>().Singleton().Use<ResourceReadingUtils>();
			For<IMediaProvider>().Singleton().Use<MediaProvider>();
		}
	}
}