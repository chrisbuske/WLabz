using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wlabz.api.accounts.savings.Model
{
    /// <summary>
    /// Saving Account Information
    /// </summary>
    public class SavingsAccount
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

        #region Constants...

        private const decimal _minimumBalance = 1000;

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
        /// Transaction List...
        /// </summary>
        public virtual IList<Transaction> Transactions { get; set; }

        #endregion

        #region Constructors...

        public SavingsAccount()
        {
            this.Transactions = new List<Transaction>();
        }

        #endregion

        #region Business Logic

        /// <summary>
        /// Validate the account creation 
        /// </summary>
        /// <param name="deposit"></param>
        /// <returns></returns>
        protected internal virtual void ValidateAccountCreation(Transaction deposit, IList<Model.TransactionType> transactionTypes)
        {

            if (AccountType != AccountTypes.Savings) throw new Exception("Only savings account interactions permissable");

            if (deposit == null || transactionTypes.Single<TransactionType>(a => a.TransactionTypeCode == deposit.TransactionType).DRCRIndicator != TransactionType.DRCRIndicators.CR|| deposit.Amount < _minimumBalance)
                throw new Exception("Require a minimum deposit of " + _minimumBalance.ToString());
        }

        protected internal virtual void ValidateWithdraw(Balance balance, Transaction withdrawal, IList<Model.TransactionType> transactionTypes)
        {

            if (AccountType != AccountTypes.Savings) throw new Exception("Only savings account interactions permissable");

            if (withdrawal == null && transactionTypes.Single<TransactionType>(a => a.TransactionTypeCode == withdrawal.TransactionType).DRCRIndicator == TransactionType.DRCRIndicators.CR)
                return;
            else if (balance.CurrentBalance(transactionTypes) - withdrawal.Amount < _minimumBalance)
                throw new Exception("Require a minimum current balance of " + _minimumBalance.ToString());
        }

        #endregion

    }
}
