using StructureMap.Configuration.DSL;

namespace Subasta.Client.Common
{
	public class RegisterIoc : Registry
	{
		public RegisterIoc()
		{
			For<IGameSimulator>().Use<GameSimulator>();
			For<IPlayer>().Use<Player>();
		}
	}
}