using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WLabz.api.accounts.current.Model
{
    /// <summary>
    /// Overdraft Information
    /// </summary>
    public class Overdraft
    {

        #region Enumerations...

        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public enum OverdraftStatuses
        {
            /// <summary>
            /// Pending 
            /// </summary>
            Pending = 1,
            /// <summary>
            /// Active
            /// </summary>
            Active = 2,
            /// <summary>
            /// Suspended
            /// </summary>
            Suspended = 4
        }

        #endregion

        #region Constants...

        private const decimal _maximumOverdraft = -100000;

        #endregion

        #region Properties...

        /// <summary>
        /// Unique Entity ID
        /// </summary>
        public virtual Guid EntityID { get; set; }
        /// <summary>
        /// Account Number
        /// </summary>
        public virtual long AccountNumber { get; set; }
        /// <summary>
        /// Overdraft Amount...
        /// </summary>      
        public virtual decimal OverdraftAmount { get; set; }
        /// <summary>
        /// Overdraft Status...
        /// </summary>
        public virtual OverdraftStatuses OverdraftStatus { get; set; }


        #endregion

        #region Business Logic

        public void ValidateOverdraftAmount(decimal amount)
        {
            if (amount > 0) amount = decimal.Multiply(amount, -1);
            if (amount > _maximumOverdraft)
                throw new Exception("Requested overdraft amount exceeds the allowed maximum overdraft of " + decimal.Multiply(_maximumOverdraft, 1).ToString());
            else
                this.OverdraftAmount = amount;
        }

        #endregion

    }
}
