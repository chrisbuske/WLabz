using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WLabz.api.accounts.current.Model
{
    /// <summary>
    /// Transaction Types associated to current account transactions
    /// </summary>
    public class TransactionType
    {
        /// <summary>
        /// Debit or Credit Types 
        /// </summary>
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public enum DRCRIndicators
        {
            /// <summary>
            /// Debit
            /// </summary>
            DR = 1,
            /// <summary>
            /// Credit
            /// </summary>
            CR = 2
        }

        /// <summary>
        /// Unique TransactionType Code for current accounts
        /// </summary>
        public virtual char TransactionTypeCode { get; set; }
        /// <summary>
        /// Description of the Transaction Type
        /// </summary>        
        public virtual string Description{ get; set; }
        /// <summary>   
        /// Debit or Credit indicator
        /// </summary>        
        public virtual DRCRIndicators DRCRIndicator { get; set; }
    }
}
