using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap.Configuration.DSL;
using Subasta.ApplicationServices;
using Subasta.DomainServices.DataAccess.Sqlite.Infrastructure;
using Subasta.DomainServices.DataAccess.Sqlite.Writters;

namespace Subasta.DomainServices.DataAccess.Sqlite.IoC
{
	public class SqliteRegistry:Registry
	{
		public SqliteRegistry()
		{
			//For<IGameSettingsStoreWritter>().Use<GameSettingsWritter>();
			For<IGameSettingsStoreWritter>().Use<NullGameSettingsWritter>();

			For<IResultStoreWritter>().Use<ResultStoreWritter>();
			//For<IQueuedResultStoreWritter>().Use<QueuedResultStoreWritter>();
			For<IQueuedResultStoreWritter>().Use<NullStoreWritter>();

			For<IGameDataHelper>().Singleton().Use<DbEngine>()
				.Ctor<bool>("inMemory").Is(x=>false)
			.Ctor<string>().Is(x=>"Games");
			For<IBootstrap>().Use<Bootstrap>();
		}

	}
}

