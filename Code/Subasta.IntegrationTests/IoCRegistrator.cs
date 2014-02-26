using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;
using Subasta.ApplicationServices;
using Subasta.DomainServices.DataAccess.Sqlite.IoC;
using Subasta.Infrastructure.ApplicationServices;
using Subasta.Infrastructure.DomainServices;

namespace Subasta.IntegrationTests
{
	public class IoCRegistrator
	{
		private static IContainer _container;
		private static bool _initialized = false;
		public static void Register()
		{
			ObjectFactory.Initialize(
				c => {c.AddRegistry<SqliteRegistry>();
				c.AddRegistry<ApplicationServicesRegistry>();
				c.AddRegistry<DomainServicesRegistry>();
				
				});

			_container = ObjectFactory.Container;
			_container.Configure(x => x.For<IContainer>().Use(_container));

			foreach (var strap in _container.GetAllInstances<IBootstrap>())
				strap.Execute();

			_initialized = true;
		}


	}
}
