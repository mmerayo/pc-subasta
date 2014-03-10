using StructureMap.Configuration.DSL;

namespace Subasta.Client.Common
{
	public class RegisterClientCommonIoc : Registry
	{
		public RegisterClientCommonIoc()
		{
			For<IGameSimulator>().Use<GameSimulator>();
			For<IPlayer>().Use<Player>();
		}
	}
}