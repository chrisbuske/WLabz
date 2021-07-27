using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using WLabz.Logs;
using WLabz.Repository;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WLabz.api.accounts.current.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OverdraftController : ControllerBase
    {

        #region Declarations...

        private readonly IRabbitLogger _logger;
        private readonly IFluentHibernateSessionManager _persistance;

        #endregion

        #region Constructors...

        public OverdraftController(IRabbitLogger logger, IFluentHibernateSessionManager persistance)
        {
            _logger = logger;
            _persistance = persistance;
        }

        #endregion

        /// <summary>
        /// Retrieve an entity...
        /// </summary>
        /// <param name="id">Account Number</param>
        /// <returns>Return the overdraft information</returns>
        [HttpGet("{accountNumber}")]
        public async Task<Model.Overdraft> Get(long accountNumber)
        {
            using ISession session = _persistance.OpenSession(System.Reflection.Assembly.GetExecutingAssembly(), false);
            using ITransaction transaction = session.BeginTransaction();
            try
            {

                // Retrieve the entity...

                Model.Overdraft overdraft = await session.GetAsync<Model.Overdraft>(accountNumber);

                await transaction.CommitAsync();

                return overdraft;

            }
            catch
            {
                if (transaction != null) await transaction.RollbackAsync();
                throw;
            }

        }

        /// <summary>
        /// Create a new overdraft...
        /// </summary>
        /// <param name="overdraft">Overdraft information to be saved</param>
        /// <returns>Recorded Overdraft</returns>
        [HttpPost]
        public async Task<Model.Overdraft> Post(Model.Overdraft overdraft)
        {

            using ISession session = _persistance.OpenSession(System.Reflection.Assembly.GetExecutingAssembly(), false);
            using ITransaction transaction = session.BeginTransaction();
            try
            {
                
                // Validate the amount requested...

                overdraft.ValidateOverdraftAmount(overdraft.OverdraftAmount);
                overdraft.OverdraftStatus = Model.Overdraft.OverdraftStatuses.Active;

               // Save the request...

                await session.SaveAsync(overdraft);

                await transaction.CommitAsync();

                _logger.PublishTransactionHistory(Guid.NewGuid(), TransactionHistory.EActions.Post, DateTime.Now, overdraft);

                return overdraft;

            }
            catch
            {
                if (transaction != null) await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Update entity information...
        /// </summary>
        /// <param name="overdraft">Overdraft inormation to be updated</param>
        /// <returns>Updated Overdraft</returns>
        [HttpPut]
        public async Task<Model.Overdraft> Put(Model.Overdraft overdraft)
        {
            using ISession session = _persistance.OpenSession(System.Reflection.Assembly.GetExecutingAssembly(), false);
            using ITransaction transaction = session.BeginTransaction();
            try
            {

                // Validate the amount requested...

                overdraft.ValidateOverdraftAmount(overdraft.OverdraftAmount);

                // Update the overdraft information...

                await session.UpdateAsync(overdraft);

                await transaction.CommitAsync();

                _logger.PublishTransactionHistory(Guid.NewGuid(), TransactionHistory.EActions.Put, DateTime.Now, overdraft);

                return overdraft;

            }
            catch
            {
                if (transaction != null) await transaction.RollbackAsync();
                throw;
            }
        }

    }
}
