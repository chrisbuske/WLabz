using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WLabz.api.clientelle.Model
{
    /// <summary>
    /// Saving Account Information
    /// </summary>
    public class Account
    {

        #region Enumerations...

        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public enum AccountTypes
        {
            /// <summary>
            /// Current Account
            /// </summary>
            Current = 1,
            /// <summary>
            /// SavingsAccount
            /// </summary>
            Savings = 2
        }

        #endregion

        #region Properties...

        /// <summary>
        /// Unique Entity ID
        /// </summary>
        public virtual Guid EntityID { get; set; }
        /// <summary>
        /// Type of account
        /// </summary>
        public virtual AccountTypes AccountType { get; set; }
        /// <summary>
        /// Unique Account Number
        /// </summary>
        public virtual long AccountNumber { get; set; }
        /// <summary>
        /// Associated Account Statements...
        /// </summary>
        public virtual IList<Statement> Statements { get; set; }

        #endregion

        #region Constructors...

        public Account()
        {
            this.Statements = new List<Statement>();
        }

        #endregion

    }
}
