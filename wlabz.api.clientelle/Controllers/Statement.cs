using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using WLabz.Logs;
using WLabz.Repository;
using System.Threading.Tasks;
using NHibernate.Criterion;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WLabz.api.clientelle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Statement : ControllerBase
    {

        #region Declarations...

        private readonly IRabbitLogger _logger;
        private readonly IFluentHibernateSessionManager _persistance;

        #endregion

        #region Constructors...

        public Statement(IRabbitLogger logger, IFluentHibernateSessionManager persistance)
        {
            _logger = logger;
            _persistance = persistance;
        }

        #endregion

        // GET api/<Statement>/{account number}
        /// <summary>
        /// Retrieve a list of all statements related to an account
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        [HttpGet("{accountNumber}")]
        public async Task<IList<Model.Statement>> Get(long accountNumber)
        {

            using ISession session = _persistance.OpenSession(System.Reflection.Assembly.GetExecutingAssembly(), true);
            using ITransaction transaction = session.BeginTransaction();
            try
            {

                IList<Model.Statement> statements = session.CreateCriteria(typeof(Model.Statement))
                    .Fetch(SelectMode.Fetch, "Statements")
                    .AddOrder(Order.Desc("StatementDate"))
                    .Add(Expression.Eq("AccountNumber", accountNumber))
                    .List<Model.Statement>();

                await transaction.CommitAsync();

                return statements;

            }
            catch
            {
                if (transaction != null) await transaction.RollbackAsync();
                throw;
            }

        }

        /// <summary>
        /// This should be run within the database as the staement should not be compiled from the outside...
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Post(Model.Statement statement)
        {

            using ISession session = _persistance.OpenSession(System.Reflection.Assembly.GetExecutingAssembly(), false);
            using ITransaction transaction = session.BeginTransaction();
            try
            {

                await session.SaveAsync(statement);                

                await transaction.CommitAsync();

                _logger.PublishTransactionHistory(Guid.NewGuid(), TransactionHistory.EActions.Post, DateTime.Now, statement);

            }
            catch
            {
                if (transaction != null) await transaction.RollbackAsync();
                throw;
            }
        }
    }
}