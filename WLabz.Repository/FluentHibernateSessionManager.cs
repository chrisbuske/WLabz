using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using NHibernate.Tool.hbm2ddl;
using Microsoft.Extensions.Options;

namespace WLabz.Repository
{

    /// <summary>
    /// Session Management for FluentHibernator Sessions....
    /// </summary>
    public class FluentHibernateSessionManager : IFluentHibernateSessionManager
    {

        #region Declarations...

        static readonly object factorylock = new object();

        private ISessionFactory _sessionFactory;
        private readonly FluentHibernateSettings _configSettings;

        #endregion

        #region Constructors...

        public FluentHibernateSessionManager(IOptions<FluentHibernateSettings> configSettings)
        {
            _configSettings = configSettings.Value;
        }

        #endregion

        /// <summary>
        /// Open a new NHibernate.Session...
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly">Assembly hosting the mappings</param>
        /// <param name="buildSchema">Flag indicating if Fluent must update the schema. This is only executed upon the initial session factory instantiation</param>
        /// <returns></returns>
        public ISession OpenSession(Assembly assembly, bool buildSchema)
        {
            if (_sessionFactory == null)
            {

                lock (factorylock)
                {

                    FluentConfiguration config;

                    if (buildSchema)
                    {
                        config = Fluently.Configure()
                            .Database(FluentNHibernate.Cfg.Db.MsSqlConfiguration.MsSql2012.ConnectionString(_configSettings.ConnectionString).DefaultSchema(_configSettings.DefaultSchema))
                            .Mappings(m => m.FluentMappings.AddFromAssembly(assembly))
                            .CurrentSessionContext(_configSettings.CurrentSessionContext)
                            .ExposeConfiguration(cfg => BuildSchema(cfg));
                    }
                    else
                    {
                        config = Fluently.Configure()
                            .Database(FluentNHibernate.Cfg.Db.MsSqlConfiguration.MsSql2012.ConnectionString(_configSettings.ConnectionString).DefaultSchema(_configSettings.DefaultSchema))
                            .Mappings(m => m.FluentMappings.AddFromAssembly(assembly))
                            .CurrentSessionContext(_configSettings.CurrentSessionContext);
                    }

                    _sessionFactory = config.BuildSessionFactory();

                }

            }

            return _sessionFactory.OpenSession();
        }

        /// <summary>
        /// Schema updater...
        /// </summary>
        /// <param name="config">NHibernate configuration containing mappings and database settings</param>
        private static void BuildSchema(Configuration config)
        {
            new SchemaUpdate(config).Execute(false, true);
        }

    }
}