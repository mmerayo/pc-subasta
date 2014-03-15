using StructureMap.Configuration.DSL;
using Subasta.Client.Common.Games;

namespace Subasta.Client.Common
{
	public class RegisterClientCommonIoc : Registry
	{
		public RegisterClientCommonIoc()
		{
			For<IGameSimulator>().Use<GameSimulator>();
			For<IStoredGameReader>().Use<StoredGameReader>();
			For<IStoredGamesCommands>().Use<StoredGamesCommands>();
			For<IPlayer>().Use<Player>();
		}
	}
}