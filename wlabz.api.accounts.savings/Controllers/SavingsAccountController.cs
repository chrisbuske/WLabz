using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using WLabz.Logs;
using WLabz.Repository;
using System.Threading.Tasks;

namespace wlabz.api.accounts.savings.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SavingsAccountController : ControllerBase
    {

        #region Declarations...

        private readonly IRabbitLogger _logger;
        private readonly IFluentHibernateSessionManager _persistance;

        #endregion

        #region Constructors...

        public SavingsAccountController(IRabbitLogger logger, IFluentHibernateSessionManager persistance)
        {
            _logger = logger;
            _persistance = persistance;
        }

        #endregion

        /// <summary>
        /// Retrieve an existing savings account information
        /// </summary>
        /// <example>GET api/<Entity>/40000000001</example>
        /// <param name="accountNumber">Account Number</param>
        /// <returns>Savings Account Information</returns>
        [HttpGet("{accountNumber}")]
        public async Task<Model.SavingsAccount> Get(long accountNumber)
        {

            using ISession session = _persistance.OpenSession(System.Reflection.Assembly.GetExecutingAssembly(), false);
            using ITransaction transaction = session.BeginTransaction();
            try
            {

                Model.SavingsAccount accountSettings = await session.GetAsync<Model.SavingsAccount>(accountNumber);

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
        /// Create a new savings account associated to an entity...
        /// </summary>
        /// <param name="savingsAccount"></param>
        /// <returns>Create Savings Account Information</returns>
        [HttpPost]
        public async Task<Model.SavingsAccount> Post(Model.SavingsAccount savingsAccount)
        {

            using ISession session = _persistance.OpenSession(System.Reflection.Assembly.GetExecutingAssembly(), false);
            using ITransaction transaction = session.BeginTransaction();
            try
            {
                                
                // Check if account number exists...

                Model.SavingsAccount accountSettingsCheck = await session.GetAsync<Model.SavingsAccount>(savingsAccount.AccountNumber);

                if (accountSettingsCheck != null) throw new Exception("Account already exists");

                // Check for an accompaning initial transaction...

                if (savingsAccount.Transactions.Count != 1) throw new Exception("Account requires an initial deposit");

                // Retrieve list of transaction types in order to check if the transaction was an account credit transaction...

                IList<Model.TransactionType> transactionTypes = session.CreateCriteria<Model.TransactionType>().List< Model.TransactionType>();
               
                // Call the savings account creation validation...

                savingsAccount.ValidateAccountCreation(savingsAccount.Transactions[0], transactionTypes);

                // Create the initial statement. This is required by the Balance to compile account balance...

                Model.Statement initialStatement = new(){ AccountNumber = savingsAccount.AccountNumber, StatementDate = DateTime.Now, ClosingBalance = 0, TotalCredits = 0, TotalDebits = 0 };

                // Save the information to the data repository...

                await session.SaveAsync(savingsAccount);

                await session.SaveAsync(initialStatement);
                savingsAccount.Transactions[0].TransactionTime = initialStatement.StatementDate;

                savingsAccount.Transactions[0].TransactionTime = DateTime.Now;

                await session.SaveAsync(savingsAccount.Transactions[0]);

                await transaction.CommitAsync();

                // Publish the events to the audit queue...

                _logger.PublishTransactionHistory(Guid.NewGuid(), TransactionHistory.EActions.Post, DateTime.Now, savingsAccount);
                _logger.PublishTransactionHistory(Guid.NewGuid(), TransactionHistory.EActions.Post, DateTime.Now, initialStatement);
                _logger.PublishTransactionHistory(Guid.NewGuid(), TransactionHistory.EActions.Post, DateTime.Now, savingsAccount.Transactions[0]);
                
                // Return the cretaed account settings...

                return savingsAccount;

            }
            catch
            {
                if (transaction != null) await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
