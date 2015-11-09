using StructureMap.Configuration.DSL;
using Subasta.ApplicationServices.Events;
using Subasta.ApplicationServices.IO;
using Subasta.Infrastructure.ApplicationServices.Events;
using Subasta.Infrastructure.ApplicationServices.IO;

namespace Subasta.Infrastructure.ApplicationServices
{
	public class ApplicationServicesRegistry : Registry
	{
		public ApplicationServicesRegistry()
		{
			For<IPathHelper>().Use<PathUtils>();
#if DEBUG
			For<IEventPublisher>().Singleton().Use<NullEventsPublisher>();
#else
			For<IEventPublisher>().Singleton().Use<ServiceEventsPublisher>();
#endif
		}
	}
}