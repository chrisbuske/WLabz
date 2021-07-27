using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using WLabz.Logs;

namespace WLabz.api.accounts.current.ServiceCollections
{
    public static class RabbitServiceCollection
    {

        private const string _configurationSection = "RabbitMQ";

        public static IServiceCollection AddRabbit(this IServiceCollection services, IConfiguration configuration)
        {
        
            services.Configure<RabbitSettings>(con => configuration.GetSection(_configurationSection).Bind(con));

            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<IPooledObjectPolicy<IModel>, RabbitModelPooledObjectPolicy>();

            services.AddSingleton<IRabbitLogger, RabbitLogger>();

            return services;
        }
    }
}
