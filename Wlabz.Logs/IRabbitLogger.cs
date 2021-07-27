using System;
using System.Collections.Generic;
using System.Text;

namespace WLabz.Logs
{
    public interface IRabbitLogger
    {
        void Publish<T>(T message, string exchangeName, string exchangeType, string routeKey)
            where T : class;

        void PublishTransactionHistory<T>(Guid session, TransactionHistory.EActions action, DateTime time, T message)
            where T : class;
    }
}
