using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wlabz.api.accounts.savings.Model
{

    /// <summary>
    /// Provides a statement for an accounting period...
    /// </summary>
    public class Balance
    {

        #region Properties...

        /// <summary>
        /// Unique Account Number
        /// </summary>
        public virtual long AccountNumber { get; set; }

        /// <summary>
        /// Unique Account Number
        /// </summary>
        public virtual DateTime StatementDate { get; set; }

        /// <summary>
        /// Closing Balance for the statement Period
        /// </summary>
        public virtual decimal ClosingBalance { get; set; }

        /// <summary>
        /// List of associate accounts...
        /// </summary>
        public virtual IList<Transaction> Transactions { get; set; }

        #endregion

        #region Constructors...

        public Balance()
        {
            this.Transactions = new List<Transaction>();
        }

        #endregion

        #region Business Logic

        /// <summary>
        /// Work out the current balance.
        /// This is derived from taking the last statement closing balance and all consecutive transactions...
        /// </summary>
        /// <returns></returns>
        protected internal virtual decimal CurrentBalance(IList<TransactionType> transactionTypes)
        {
            decimal _runningTotal = this.ClosingBalance;

            foreach (Transaction transaction in this.Transactions)
            {
                if (transaction != null && transaction.AccountNumber == this.AccountNumber && transaction.TransactionTime >= this.StatementDate)
                {                  
                    if (transactionTypes.Single<TransactionType>(a => a.TransactionTypeCode == transaction.TransactionType).DRCRIndicator == TransactionType.DRCRIndicators.CR)
                    {
                        _runningTotal += transaction.Amount;
                    }
                    else
                        _runningTotal -= transaction.Amount;
                }
            }

            return _runningTotal;

        }

        #endregion

        #region Overrides...

        public override bool Equals(object obj)
        {
            if (obj is not Balance other) return false;
            if (ReferenceEquals(this, other)) return true;

            return this.AccountNumber == other.AccountNumber &&
                this.StatementDate == other.StatementDate;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = GetType().GetHashCode();
                hash = (hash * 31) ^ AccountNumber.GetHashCode();
                hash = (hash * 31) ^ StatementDate.GetHashCode();

                return hash;
            }
        }

        #endregion
    }
}
