using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace WLabz.Logs
{
    /// <summary>
    /// 
    /// </summary>
    public class RabbitModelPooledObjectPolicy : IPooledObjectPolicy<IModel>
    {

        private readonly RabbitSettings _configSettings;

        private readonly IConnection _connection;

        public RabbitModelPooledObjectPolicy(IOptions<RabbitSettings> configSettings)
        {
            _configSettings = configSettings.Value;
            _connection = GetConnection();
        }

        private IConnection GetConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configSettings.HostName,
                UserName = _configSettings.UserName,
                Password = _configSettings.Password,
                Port = _configSettings.Port,
                VirtualHost = _configSettings.VHost,
            };

            try
            {
                return factory.CreateConnection();
            }
            catch
            {
                return null;
            }
        }

        public IModel Create()
        {
            return _connection.CreateModel();
        }

        public bool Return(IModel obj)
        {
            if (obj.IsOpen)
            {
                return true;
            }
            else
            {
                obj?.Dispose();
                return false;
            }
        }
    }
}