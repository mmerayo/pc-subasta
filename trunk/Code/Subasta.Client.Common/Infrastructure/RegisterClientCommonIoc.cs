using StructureMap.Configuration.DSL;
using Subasta.Client.Common.Game;
using Subasta.Client.Common.Storage;
using Subasta.DomainServices.Dal;

namespace Subasta.Client.Common.Infrastructure
{
	public class RegisterClientCommonIoc : Registry
	{
		public RegisterClientCommonIoc()
		{
			For<IGameSimulator>().Use<GameSimulator>();
			For<IGameSet>().Use<GameSet>();
			For<IStoredGameReader>().Use<StoredGameReader>();
			For<IStoredGamesCommands>().Use<StoredGamesCommands>();
		}
	}
}