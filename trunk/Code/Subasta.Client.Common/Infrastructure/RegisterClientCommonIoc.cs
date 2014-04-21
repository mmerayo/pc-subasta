﻿using StructureMap.Configuration.DSL;
using Subasta.Client.Common.Game;
using Subasta.Client.Common.Storage;
using Subasta.DomainServices.Dal;

namespace Subasta.Client.Common.Infrastructure
{
	public class RegisterClientCommonIoc : Registry
	{
		public RegisterClientCommonIoc()
		{
			For<IGameHandler>().Use<GameHandler>();
			For<IGameSetHandler>().Use<GameSetHandler>();
			For<IStoredGameReader>().Use<StoredGameReader>();
			For<IStoredGamesCommands>().Use<StoredGamesCommands>();
		}
	}
}