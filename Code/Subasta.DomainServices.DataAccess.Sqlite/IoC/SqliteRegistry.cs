using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap.Configuration.DSL;
using Subasta.DomainServices.DataAccess.Sqlite.Writters;

namespace Subasta.DomainServices.DataAccess.Sqlite.IoC
{
	class SqliteRegistry:Registry
	{
		public SqliteRegistry()
		{
			For<IGameSettingsStoreWritter>().Use<GameSettingsWritter>();
			For<IResultStoreWritter>().Use<ResultStoreWritter>();
			For<IGameDataHelper>().Singleton().Use<DbEngine>();
		}

	}
}
