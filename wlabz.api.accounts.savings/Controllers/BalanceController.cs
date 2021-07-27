using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NHibernate;
using WLabz.Logs;
using WLabz.Repository;
using System.Threading.Tasks;
using NHibernate.Criterion;

namespace wlabz.api.accounts.savings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        #region Declarations...

        private readonly IFluentHibernateSessionManager _persistance;

        #endregion

        #region Constructors...

        public BalanceController(IFluentHibernateSessionManager persistance)
        {
            _persistance = persistance;
        }

        #endregion

        /// <summary>
        /// Retrieve the current balance for a savings account...
        /// </summary>
        /// <param name="accountNumber">Accoun Number of the account</param>
        /// <example>GET api/<Statement>/{account number}}</example>
        /// <returns>Current Account Balance</returns>
        [HttpGet("{accountNumber}")]
        public async Task<decimal> Get(long accountNumber)
        {

            using NHibernate.ISession session = _persistance.OpenSession(System.Reflection.Assembly.GetExecutingAssembly(), false);
            using ITransaction transaction = session.BeginTransaction();
            try
            {

                // Retrieve the latest statement. This is required to calculated the current balance ..

                Model.Balance balance = session.CreateCriteria(typeof(Model.Balance)).SetMaxResults(1)
                    .Fetch(SelectMode.Fetch, "Statements").AddOrder(Order.Desc("StatementDate"))
                    .Add(Expression.Eq("AccountNumber", accountNumber))
                    .UniqueResult<Model.Balance>();

                if (balance == null)
                {
                    balance = new Model.Balance
                    {
                        ClosingBalance = 0
                    };
                }

                // Retrieve a list of the trnsaction types...

                IList<Model.TransactionType> transactionTypes = session.CreateCriteria<Model.TransactionType>().List<Model.TransactionType>();

                // Retrieve a list of all transactions that have taken place after the latest statement was produced...

                balance.Transactions = session.CreateCriteria(typeof(Model.Transaction))
                    .Fetch(SelectMode.Fetch, "Transactions").AddOrder(Order.Asc("TransactionTime"))
                    .Add(Expression.Eq("AccountNumber", accountNumber))
                    .Add(Expression.Ge("TransactionTime", balance.StatementDate))
                    .List<Model.Transaction>();                                

                await transaction.CommitAsync();

                // Calculate and return the current balance from the business logic...

                return balance.CurrentBalance(transactionTypes);

            }
            catch
            {
                if (transaction != null) await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
