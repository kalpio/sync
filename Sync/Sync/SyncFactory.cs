using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Context;

namespace Sync
{
    internal static class SyncFactory
    {
        private static ISession _session;
        private static ISessionFactory _sessionFactory;

        public static ISession Get()
        {
            if (_session == null || !_session.IsOpen) {
                if (_sessionFactory == null) {
                    _sessionFactory = buildSessionFactory();
                }

                _session = _sessionFactory.OpenSession();
            }

            return _session;
        }

        static void buildSchema(NHibernate.Cfg.Configuration config)
        {
            new SchemaUpdate(config).Execute(false, true);
            new SchemaExport(config).Create(false, false);
            //config.SetInterceptor(new SqlStatementInterceptor());
        }

        private static ISessionFactory buildSessionFactory()
        {
            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard
                .ShowSql()
                .UsingFile(Settings.GetDbFilePath()))
                .Mappings(m => m
                    .FluentMappings.AddFromAssemblyOf<DataAccess.Backup>())
                .ExposeConfiguration(buildSchema)
                .BuildSessionFactory();
        }
    }
}
