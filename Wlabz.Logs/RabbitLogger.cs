using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using System;
using System.Text;
using Microsoft.Extensions.Options;

namespace WLabz.Logs
{

    /// <summary>
    /// 
    /// </summary>
    public class RabbitLogger : IRabbitLogger
    {

        #region Declarations...

        private readonly DefaultObjectPool<IModel> _objectPool;
        private readonly RabbitSettings  _configSettings;

        #endregion

        #region Constructor...

        public RabbitLogger(IPooledObjectPolicy<IModel> objectPolicy, IOptions<RabbitSettings> configSettings)
        {
            _objectPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount * 2);
            _configSettings = configSettings.Value;
        }

        #endregion

        public void Publish<T>(T message, string exchangeName, string exchangeType, string routeKey)
            where T : class
        {
            if (message == null)
                return;

            var channel = _objectPool.Get();

            try
            {
                channel.ExchangeDeclare(exchangeName, exchangeType, true, false, null);

                var sendBytes = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(message));

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;



                channel.BasicPublish(exchangeName, routeKey, properties, sendBytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _objectPool.Return(channel);
            }
        }

        /// <summary>
        /// Publish the transaction history to a rabbit exchage...
        /// E
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session">Session issuing the transaction</param>
        /// <param name="entity">Entity type against which the transaction is audited</param>
        /// <param name="action">Type of action taken</param>
        /// <param name="time">Time of the actions</param>
        /// <param name="message">Content of the entity</param>
        public void PublishTransactionHistory<T>(Guid session, TransactionHistory.EActions action, DateTime time, T message)
            where T : class
        {
            if (message == null)
                return;

            var channel = _objectPool.Get();

            try
            {

               // channel.ExchangeDeclare(_configSettings.Exchange, _configSettings.ExchangeType, true, false, null);

                var transHistory = new TransactionHistory(session, message.GetType(), action, time, System.Text.Json.JsonSerializer.Serialize(message));
                var sendBytes = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(transHistory));
                var properties = channel.CreateBasicProperties();

                properties.Persistent = true;

                channel.BasicPublish(_configSettings.TransactionHistoryExchange, _configSettings.TransactionHistoryRouteKey, properties, sendBytes);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _objectPool.Return(channel);
            }
        }

    }
}