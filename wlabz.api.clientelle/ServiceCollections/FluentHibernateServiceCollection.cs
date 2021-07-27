using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using WLabz.Repository;

namespace WLabz.api.clientelle.ServiceCollections
{

    /// <summary>
    /// Service Extension Class for FluentHibernate Persistance...
    /// </summary>
    public static class FluentHibernateServiceCollection
    {

        #region Declarations...

        /// <summary>
        /// appsettings.json Key...
        /// </summary>
        private const string _configurationSection = "FluentHibernate";

        #endregion


        public static IServiceCollection AddFluentHibernate(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<FluentHibernateSettings>(con => configuration.GetSection(_configurationSection).Bind(con));
            services.AddSingleton<IFluentHibernateSessionManager, FluentHibernateSessionManager>();

            return services;

            //var mapper = new ModelMapper();
            //mapper.AddMappings(typeof(NHibernateExtensions).Assembly.ExportedTypes);
            //HbmMapping domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            //var configuration = new Configuration();
            //configuration.DataBaseIntegration(c =>
            //{
            //    c.Dialect<MsSql2012Dialect>();
            //    c.ConnectionString = connectionString;
            //    c.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
            //    c.SchemaAction = SchemaAutoAction.Validate;
            //    c.LogFormattedSql = true;
            //    c.LogSqlInConsole = true;
            //});
            //configuration.AddMapping(domainMapping);

            //var sessionFactory = configuration.BuildSessionFactory();

            //services.AddSingleton(sessionFactory);
            //services.AddScoped(factory => sessionFactory.OpenSession());
            //services.AddScoped<IMapperSession, NHibernateMapperSession>();

            //return services;
        }

        //static readonly object factorylock = new object();

        //private static ISessionFactory _sessionFactory;
        //private const string _persistanceConnectionString = "ConnectionStrings:DefaultConnection";

        //public ISession OpenSession()
        //{
        //    if (_sessionFactory == null)
        //    {

        //        lock (factorylock)
        //        {

        //            var config = Fluently.Configure()
        //            .Database(MsSqlConfiguration.MsSql2012.ConnectionString(Startup.Configuration[_persistanceConnectionString]))
        //            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Program>())
        //            .CurrentSessionContext("call")
        //            .ExposeConfiguration(cfg => BuildSchema(cfg));

        //            _sessionFactory = config.BuildSessionFactory();


        //        }

        //    }

        //    return _sessionFactory.OpenSession();

        //}

        //private static void BuildSchema(Configuration config)
        //{
        //    //var sessionSource = new SessionSource(config);
        //    //var session = sessionSource.CreateSession();
        //    //new SchemaUpdate(config).Execute(false, update);
        //    //sessionSource.BuildSchema(session);
        //    new SchemaUpdate(config).Execute(false, true);

        //}

    }
}
