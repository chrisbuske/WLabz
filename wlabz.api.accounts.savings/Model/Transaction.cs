using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wlabz.api.accounts.savings.Model
{
    public class Transaction
    {

        #region Properties...

        /// <summary>
        /// Account Identifier
        /// </summary>
        public virtual long AccountNumber { get; set; }
        /// <summary>
        /// Date and time of Transaction
        /// </summary>
        public virtual DateTime TransactionTime { get; set; }
        /// <summary>
        /// Type of Transaction
        /// </summary>
        public virtual char TransactionType {get;set;}
        /// <summary>
        /// Description of the transaction
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// Transaction Amount
        /// </summary>
        public virtual decimal Amount { get; set; }

        #endregion

        #region Overrides...

        public override bool Equals(object obj)
        {
            if (obj is not Transaction other) return false;
            if (ReferenceEquals(this, other)) return true;

            return this.AccountNumber == other.AccountNumber &&
                this.TransactionTime == other.TransactionTime;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = GetType().GetHashCode();
                hash = (hash * 31) ^ AccountNumber.GetHashCode();
                hash = (hash * 31) ^ TransactionTime.GetHashCode();

                return hash;
            }
        }

        #endregion

    }
}
