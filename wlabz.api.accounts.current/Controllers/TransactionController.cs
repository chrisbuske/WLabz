using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using WLabz.Logs;
using WLabz.Repository;
using System.Threading.Tasks;
using NHibernate.Criterion;

namespace WLabz.api.accounts.current.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {

        #region Declarations...

        private readonly IRabbitLogger _logger;
        private readonly IFluentHibernateSessionManager _persistance;

        #endregion

        #region Constructors...

        public TransactionController(IRabbitLogger logger, IFluentHibernateSessionManager persistance)
        {
            _logger = logger;
            _persistance = persistance;
        }

        #endregion

        /// <summary>
        /// Manage a current account transaction...
        /// </summary>
        /// <param name="accountTransaction"></param>
        /// <returns>Completed Transaction with updated Transaction Date</returns>
        [HttpPost]
        public async Task<Model.Transaction> Post(Model.Transaction accountTransaction)
        {

            using ISession session = _persistance.OpenSession(System.Reflection.Assembly.GetExecutingAssembly(), false);
            using ITransaction transaction = session.BeginTransaction();
            try
            {

                // Check if account is valid...
                //TODO: Account Entity owner should be checked vesus the session owner...

                Model.CurrentAccount accountSettings = await session.GetAsync<Model.CurrentAccount>(accountTransaction.AccountNumber);

                if (accountSettings.AccountType == Model.CurrentAccount.AccountTypes.Current)
                {

                    // Retrieve a list of transaction types and check transaction type is valid...

                   IList< Model.TransactionType> transactionTypes = session.CreateCriteria<Model.TransactionType>().List<Model.TransactionType>();


                    if (transactionTypes.Single<Model.TransactionType>(a => a.TransactionTypeCode == accountTransaction.TransactionType).DRCRIndicator == Model.TransactionType.DRCRIndicators.DR)
                    {

                        DateTime currentTime = DateTime.Now;

                        // Get current balance. To do this we must get the latest statement and all proceeding transactions after the statement...

                        Model.Balance balance = session.CreateCriteria(typeof(Model.Balance)).SetMaxResults(1)
                           .Fetch(SelectMode.Fetch, "Statements").AddOrder(Order.Desc("StatementDate"))
                           .Add(Expression.Eq("AccountNumber", accountTransaction.AccountNumber))
                           .UniqueResult<Model.Balance>();

                        if (balance == null)
                        {
                            balance = new Model.Balance
                            {
                                StatementDate = DateTime.MinValue,
                                ClosingBalance = 0
                            };
                        }

                        balance.Transactions = session.CreateCriteria(typeof(Model.Transaction))
                            .Fetch(SelectMode.Fetch, "Transactions").AddOrder(Order.Asc("TransactionTime"))
                            .Add(Expression.Eq("AccountNumber", accountTransaction.AccountNumber))
                            .Add(Expression.Ge("TransactionTime", balance.StatementDate))
                            .List<Model.Transaction>();


                        // Fetch the overdraft facility...

                        Model.Overdraft overdraft = await session.GetAsync<Model.Overdraft>(accountTransaction.AccountNumber);

                        // Call business logic to check if transaction is a withdrawl, and if so if it may be performed...
                        // This ensures current account minimum balance is adhered to...

                        accountSettings.ValidateWithdraw(balance, accountTransaction, transactionTypes, overdraft);

                        // If no errors have occured then update the transaction time and record the transaction...
                        // All transactions are recorded as positive values against their transaction type which indicates if the transaction is an account debit or account credit...

                        accountTransaction.TransactionTime = DateTime.Now;

                        await session.SaveAsync(accountTransaction);

                    }
                    else
                    {

                        accountTransaction.TransactionTime = DateTime.Now;

                        await session.SaveAsync(accountTransaction);

                    }

                    await transaction.CommitAsync();

                    // Publish the event to the audit queue...

                    _logger.PublishTransactionHistory(Guid.NewGuid(), TransactionHistory.EActions.Post, DateTime.Now, accountTransaction);

                    return accountTransaction;

                }
                else
                {
                    throw new Exception("Invalid account type");
                }
            }
            catch
            {
                if (transaction != null) await transaction.RollbackAsync();
                throw;
            }
        }
    }
}