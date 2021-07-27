using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using WLabz.Logs;
using WLabz.Repository;
using System.Threading.Tasks;

namespace WLabz.api.accounts.current.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CurrenAccountController : ControllerBase
    {

        #region Declarations...

        private readonly IRabbitLogger _logger;
        private readonly IFluentHibernateSessionManager _persistance;

        #endregion

        #region Constructors...

        public CurrenAccountController(IRabbitLogger logger, IFluentHibernateSessionManager persistance)
        {
            _logger = logger;
            _persistance = persistance;
        }

        #endregion

        /// <summary>
        /// Retrieve an existing current account information
        /// </summary>
        /// <example>GET api/<Entity>/40000000001</example>
        /// <param name="accountNumber">Account Number</param>
        /// <returns>Current Account Information</returns>
        [HttpGet("{accountNumber}")]
        public async Task<Model.CurrentAccount> Get(long accountNumber)
        {

            using ISession session = _persistance.OpenSession(System.Reflection.Assembly.GetExecutingAssembly(), false);
            using ITransaction transaction = session.BeginTransaction();
            try
            {

                Model.CurrentAccount accountSettings = await session.GetAsync<Model.CurrentAccount>(accountNumber);

                await transaction.CommitAsync();

                return accountSettings;

            }
            catch
            {
                if (transaction != null) await transaction.RollbackAsync();
                throw;
            }

        }

        /// <summary>
        /// Create a new current account associated to an entity...
        /// </summary>
        /// <param name="currentAccount"></param>
        /// <returns>Create Current Account Information</returns>
        [HttpPost]
        public async Task<Model.CurrentAccount> Post(Model.CurrentAccount currentAccount)
        {

            using ISession session = _persistance.OpenSession(System.Reflection.Assembly.GetExecutingAssembly(), false);
            using ITransaction transaction = session.BeginTransaction();
            try
            {
                                
                // Check if account number exists...

                Model.CurrentAccount accountSettingsCheck = await session.GetAsync<Model.CurrentAccount>(currentAccount.AccountNumber);

                if (accountSettingsCheck != null) throw new Exception("Account already exists");

                // Check for an accompaning initial transaction...

                if (currentAccount.Transactions.Count != 1) throw new Exception("Account requires an initial deposit");

                // Retrieve list of transaction types in order to check if the transaction was an account credit transaction...

                IList<Model.TransactionType> transactionTypes = session.CreateCriteria<Model.TransactionType>().List< Model.TransactionType>();
               
                // Call the curren account creation validation...

                currentAccount.ValidateAccountCreation(currentAccount.Transactions[0], transactionTypes);

                // Create the initial statement. This is required by the Balance to compile account balance...

                Model.Statement initialStatement = new(){ AccountNumber = currentAccount.AccountNumber, StatementDate = DateTime.Now, ClosingBalance = 0, TotalCredits = 0, TotalDebits = 0 };

                // Save the information to the data repository...

                await session.SaveAsync(currentAccount);

                await session.SaveAsync(initialStatement);
                currentAccount.Transactions[0].TransactionTime = initialStatement.StatementDate;

                currentAccount.Transactions[0].TransactionTime = DateTime.Now;

                await session.SaveAsync(currentAccount.Transactions[0]);

                await transaction.CommitAsync();

                // Publish the events to the audit queue...

                _logger.PublishTransactionHistory(Guid.NewGuid(), TransactionHistory.EActions.Post, DateTime.Now, currentAccount);
                _logger.PublishTransactionHistory(Guid.NewGuid(), TransactionHistory.EActions.Post, DateTime.Now, initialStatement);
                _logger.PublishTransactionHistory(Guid.NewGuid(), TransactionHistory.EActions.Post, DateTime.Now, currentAccount.Transactions[0]);
                
                // Return the cretaed account settings...

                return currentAccount;

            }
            catch
            {
                if (transaction != null) await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
