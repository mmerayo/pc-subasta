using System;
using System.Collections.Generic;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Subasta.DomainServices.DataAccess.Sqlite.Models;

namespace Subasta.DomainServices.DataAccess.Sqlite
{
    static class SessionFactoryProvider
    {
        private static readonly Dictionary<Guid, ISessionFactory> _sessionFactories=new Dictionary<Guid, ISessionFactory>();
        private static readonly Dictionary<Guid, Configuration> _configurations = new Dictionary<Guid, Configuration>();
        public static ISessionFactory GetSessionFactory(Guid gameId, string connectionString)
        {
            if(!_sessionFactories.ContainsKey(gameId))
            {
                Configuration cfg;
                _sessionFactories.Add(gameId, BuildSessionFactory(connectionString,out cfg));
                _configurations.Add(gameId,cfg);
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
            Configuration config=null;
            var fluentConfiguration=Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.ConnectionString(connectionString))
                .Mappings(m => m.AutoMappings.Add(AutoMap.AssemblyOf<GameInfo>(new DefaultAutomappingConfiguration())));
            var result= fluentConfiguration.ExposeConfiguration(x => config = x).BuildSessionFactory();
            cfg = config;
            return result;

        }

        public static void CreateSchema(Guid gameId)
        {
#if DEBUG
            new SchemaUpdate (_configurations[gameId]).Execute(true,true);
            //TODO: GENERATE THE DB FROM SCRIPT after improvement
#endif

        }
    }
}