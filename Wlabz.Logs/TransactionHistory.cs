using System;
using RabbitMQ.Client;

namespace WLabz.Logs
{

    /// <summary>
    /// Transaction Classhosts transaction information to be placed onto the transaction queue for adit history storage.
    /// The to string is overridden to provide a JSon resresentation of the transaction record, thus making it easy to stored in a NoSQL database.
    /// </summary>
    public class TransactionHistory
    {

        #region Enumerations

        public enum EActions : byte
        {
            /// <summary>
            /// Create new record...
            /// </summary>
            Post = 1,
            /// <summary>
            /// Update a record...
            /// </summary>
            Put = 2,
            /// <summary>
            /// Delete the record...
            /// </summary>
            Delete = 4,
            /// <summary>
            /// Administrative funtion called...
            /// </summary>
            Admin = 5
        }


        #endregion

        #region Properties

        /// <summary>
        /// Session identifier
        /// </summary>
        public Guid Session { get; private set; }
        /// <summary>
        /// Model Entity Type
        /// </summary>
        public string Entity { get; private set; }
        /// <summary>
        /// Action committed
        /// </summary>
        public EActions Action { get; private set; }
        /// <summary>
        /// Time of the action
        /// </summary>
        public DateTime Time { get; private set; }
        /// <summary>
        /// Associted information submitted
        /// </summary>
        public string Record { get; private set; }

        #endregion

        #region Constructors

        public TransactionHistory(Guid session, Type entity, EActions action, DateTime time, string record)
        {
            Session = session;
            Entity = entity.FullName;
            Action = action;
            Time = time;
            Record = record;
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }

        #endregion

    }
}
