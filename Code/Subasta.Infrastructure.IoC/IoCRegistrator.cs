using System;
using System.IO;
using StructureMap;
using Subasta.ApplicationServices;
using Subasta.DomainServices.DataAccess.Sqlite.IoC;
using Subasta.Infrastructure.ApplicationServices;
using Subasta.Infrastructure.Domain;
using Subasta.Infrastructure.DomainServices;

namespace Subasta.Infrastructure.IoC
{
	public class IoCRegistrator
	{
		private static IContainer _container;
		private static bool _initialized = false;
		public static void Register()
		{

			if (_initialized) return;

			ObjectFactory.Initialize(
				c => {c.AddRegistry<SqliteRegistry>();
				c.AddRegistry<ApplicationServicesRegistry>();
				c.AddRegistry<DomainServicesRegistry>();
					     c.AddRegistry<DomainRegistry>();

				});

			_container = ObjectFactory.Container;
			_container.Configure(x => x.For<IContainer>().Use(_container));

			foreach (var strap in _container.GetAllInstances<IBootstrap>())
				strap.Execute();

			_initialized = true;

#if DEBUG
			ExtractInjectionReport();
#endif
		}

		public static void ExtractInjectionReport()
		{
			
				//Do not log into the bin folder as forces recompilation of the app(shadow copy)
				var directoryName = Path.Combine(AppDomain.CurrentDomain.GetData("APPBASE").ToString(), "DebugReports");

				if (!Directory.Exists(directoryName))
					Directory.CreateDirectory(directoryName);

				var fileName = Path.Combine(directoryName, "InjectionsReport.txt");
				var reportText = ObjectFactory.WhatDoIHave();

				File.WriteAllText(fileName, reportText);
			
		}


	}
}
