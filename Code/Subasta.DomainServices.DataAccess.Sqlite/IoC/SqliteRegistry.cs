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
			For<IGameSettingsStoreWritter>().Use<GameSettingsWritter>();
			For<IResultStoreWritter>().Use<ResultStoreWritter>();
			For<IGameDataHelper>().Singleton().Use<DbEngine>();
			For<IBootstrap>().Use<Bootstrap>();
		}

	}
}
