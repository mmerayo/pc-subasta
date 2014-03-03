using System;
using System.Collections.Generic;
using System.IO;
using StructureMap;
using StructureMap.Configuration.DSL;
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
		private static readonly object _syncLock=new object();

		public static void Register(List<Registry> clientRegistries=null )
		{

			if (!_initialized)
				lock (_syncLock)
					if (!_initialized)
					{

						ObjectFactory.Initialize(
							c =>
								{
									c.AddRegistry<SqliteRegistry>();
									c.AddRegistry<ApplicationServicesRegistry>();
									c.AddRegistry<DomainServicesRegistry>();
									c.AddRegistry<DomainRegistry>();
									if (clientRegistries != null)
										clientRegistries.ForEach(c.AddRegistry);
								});

						_container = ObjectFactory.Container;
						_container.Configure(x => x.For<IContainer>().Use(_container));

						foreach (var strap in _container.GetAllInstances<IBootstrap>())
							strap.Execute();


#if DEBUG
						ExtractInjectionReport();
#endif
						_initialized = true;

					}
		}

		public static
			void ExtractInjectionReport()
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
