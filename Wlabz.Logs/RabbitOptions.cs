using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WLabz.Logs
{
    /// <summary>
    /// Settings for connecting to Rabbit...
    /// </summary>
    public class RabbitSettings
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string HostName { get; set; }

        public int Port { get; set; } 

        public string VHost { get; set; } 

        public string TransactionHistoryExchange { get; set; }

        public string TransactionHistoryExchangeType { get; set; } 

        public string TransactionHistoryRouteKey { get; set; } 
    }
}
