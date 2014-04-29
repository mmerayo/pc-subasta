using StructureMap.Configuration.DSL;
using Subasta.DomainServices.Factories;
using Subasta.Infrastructure.DomainServices.Factories;

namespace Subasta.Infrastructure.DomainServices
{
	public class DomainServicesRegistry : Registry
	{
		public DomainServicesRegistry()
		{
			//TODO: REMOVE THIS DEPENDENCY
			//For<ISuit>().Use(Suit.Suits.First());
			For<IPlayerFactory>().Use<PlayerFactory>();
		}
	}
}