using StructureMap.Configuration.DSL;
using Subasta.Client.Common.Games;
using Subasta.DomainServices.Dal;

namespace Subasta.Client.Common
{
	public class RegisterClientCommonIoc : Registry
	{
		public RegisterClientCommonIoc()
		{
			For<IGameSimulator>().Use<GameSimulator>();
			For<IStoredGameReader>().Use<StoredGameReader>();
			For<IStoredGamesCommands>().Use<StoredGamesCommands>();
		}
	}
}