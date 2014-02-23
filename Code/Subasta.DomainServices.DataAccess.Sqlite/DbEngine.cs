using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Threading;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Subasta.ApplicationServices;
using Subasta.DomainServices.DataAccess.Sqlite.Mappings;
using Subasta.DomainServices.DataAccess.Sqlite.Models;

namespace Subasta.DomainServices.DataAccess.Sqlite
{
	internal class DbEngine : IGameDataHelper, IDisposable
	{
		private readonly IPathHelper _pathHelper;
		private readonly string _dbFolderPath;
		protected bool InMemory { get; private set; }
		private readonly object _syncLock = new object();
		private string DbName { get; set; }
		private SQLiteConnection _connection = null; //holds the connection in memory


		public DbEngine(IPathHelper pathHelper, bool inMemory,string dbFolderPath)
		{
			_pathHelper = pathHelper;
			_dbFolderPath = dbFolderPath;

			InMemory = inMemory;
		}

		public bool Created { get; private set; }

		private string FilePath
		{
			get
			{
				if (!InMemory)
					if (string.IsNullOrEmpty(_dbFolderPath))
						return _pathHelper.GetApplicationFolderPathForFile(DbName);
					else
						return Path.Combine(_dbFolderPath, DbName);
				throw new InvalidOperationException("In memory dont have a file");
			}
		}

		public void CreateDatabase(Guid gameId)
		{
		    //its created automatically
		    //DropDatabase(); //to keep the same behavior as sqlserver //TODO: THIS MIGHT NEED TO BE REMOVED
		    Debug.Assert(gameId != Guid.Empty);

		    SetDbName(gameId);

		    if (!Created)
		        lock (_syncLock)
		            if (!Created)
		            {
		                Debug.Assert(_connection == null);
		                _connection = new SQLiteConnection(GetConnectionString());
		                _connection.Open();

		                CreateSchema(gameId);
                        Created = true;
		            }

		    
		}

	    private  void SetDbName(Guid gameId)
	    {
            DbName = string.Format("db{0}.game", gameId.ToString().Replace('-', '_'));
	    }

	    private void CreateSchema(Guid gameId)
	    {
	        SessionFactoryProvider.GetSessionFactory(gameId, _connection.ConnectionString);
	        SessionFactoryProvider.CreateSchema(gameId);
	    }

		public void DropDatabase()
		{
			lock (_syncLock)
			{
				if (!InMemory)
				{
					DeleteDbFile(3);
					Created = false;
					//DataBase = null;

				}
				else
				{
					if (_connection != null)
					{
						_connection.Dispose();
						_connection = null;
					}
				}
			}
		}

		private void DeleteDbFile(int tries)
		{
			if (tries == 0)
				return;
			try
			{
				if (File.Exists(FilePath))
					File.Delete(FilePath);
			}
			catch (Exception)
			{
				Thread.Sleep(150);
				DeleteDbFile(--tries);
			}
		}

		public string GetConnectionString()
		{
			return !InMemory
							   ? string.Format("Data Source={0};Version=3;BinaryGuid=False;", FilePath)
							   : string.Format("FullUri=file:{0}?mode=memory&cache=shared;Version=3;BinaryGuid=False;", DbName);
		}

	    public IUnitOfWork<TSession> GetUnitOfWork<TSession>(Guid gameId)
	    {
            if (!(typeof(TSession) is ISession))
                throw new NotSupportedException();
            SetDbName(gameId);
	        return (IUnitOfWork<TSession>) new NHibernateUnitOfWork(SessionFactoryProvider.GetSessionFactory(gameId,GetConnectionString()));
	    }

	    public void ReleaseResources(Guid gameId)
	    {

	        SessionFactoryProvider.ReleaseDbConfiguration(gameId);
	    }

	    private bool _disposed = false;

		public void Dispose()
		{
			if (!_disposed)
				lock (_syncLock)
					if (!_disposed)
					{
						DropDatabase();
						_disposed = true;
					}

		}

        private static class SessionFactoryProvider
        {
            private static readonly Dictionary<Guid, ISessionFactory> _sessionFactories = new Dictionary<Guid, ISessionFactory>();
            private static readonly Dictionary<Guid, Configuration> _configurations = new Dictionary<Guid, Configuration>();
            public static ISessionFactory GetSessionFactory(Guid gameId, string connectionString)
            {
                if (!_sessionFactories.ContainsKey(gameId))
                {
                    Configuration cfg;
                    _sessionFactories.Add(gameId, BuildSessionFactory(connectionString, out cfg));
                    _configurations.Add(gameId, cfg);
                }
                return _sessionFactories[gameId];
            }


            public static void ReleaseDbConfiguration(Guid gameId)
            {
                if (!_sessionFactories.ContainsKey(gameId))
                    return;
                _sessionFactories.Remove(gameId);
                _configurations.Remove(gameId);
            }

            private static ISessionFactory BuildSessionFactory(string connectionString, out Configuration cfg)
            {
                Configuration config = null;
                var fluentConfiguration = Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard.ConnectionString(connectionString))
                    .Mappings(m => m.AutoMappings.Add(AutoMap.AssemblyOf<GameInfo>(new MyAutomappingConfiguration())));
                var result = fluentConfiguration.ExposeConfiguration(x => config = x).BuildSessionFactory();
                cfg = config;
                return result;

            }

            public static void CreateSchema(Guid gameId)
            {
#if DEBUG
                new SchemaUpdate(_configurations[gameId]).Execute(true, true);
                //TODO: GENERATE THE DB FROM SCRIPT after improvement
#endif

            }

            private class MyAutomappingConfiguration:DefaultAutomappingConfiguration
            {
                public override bool ShouldMap(Type type)
                {
                    return type.Namespace == typeof (CardInfo).Namespace;
                }
            }

        }
	}
}
