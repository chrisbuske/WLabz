using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WLabz.api.accounts.current.Model
{
    /// <summary>
    /// Current Account Information
    /// </summary>
    public class CurrentAccount
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
        /// Transaction List...
        /// </summary>
        public virtual IList<Transaction> Transactions { get; set; }

        #endregion

        #region Constructors...

        public CurrentAccount()
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

            if (AccountType != AccountTypes.Current) throw new Exception("Only current account interactions permissable");

        }

        protected internal virtual void ValidateWithdraw(Balance balance, Transaction withdrawal, IList<Model.TransactionType> transactionTypes, Overdraft overdraft)
        {

            if (AccountType != AccountTypes.Current) throw new Exception("Only current account interactions permissable");

            if (withdrawal == null || transactionTypes.Single<TransactionType>(a => a.TransactionTypeCode == withdrawal.TransactionType).DRCRIndicator == TransactionType.DRCRIndicators.CR)
                return;
            else if((overdraft.OverdraftStatus != Overdraft.OverdraftStatuses.Active && balance.CurrentBalance(transactionTypes) - withdrawal.Amount > 0) ||
                (balance.CurrentBalance(transactionTypes) - withdrawal.Amount < overdraft.OverdraftAmount))
                throw new Exception("Withdrawal  Request Declined. The withdrawal amount exceeds the available balance.");
        }

        #endregion

    }
}
