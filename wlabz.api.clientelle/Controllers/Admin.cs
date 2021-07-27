using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using WLabz.Logs;
using WLabz.Repository;
using System.Threading.Tasks;

namespace wlabz.api.clientelle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Admin : ControllerBase
    {
        #region Declarations...

        private readonly IFluentHibernateSessionManager _persistance;

        #endregion

        #region Constructors...

        public Admin(IFluentHibernateSessionManager persistance)
        {
            _persistance = persistance;
        }

        #endregion


        /// <summary>
        /// Execute administrive tasks...
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Post(Model.Admin admin)
        {
            if (admin != null)
            {

                if (admin.AdminRequest == Model.Admin.AdminRequests.SetupRepository)
                {
                    using ISession session = _persistance.OpenSession(System.Reflection.Assembly.GetExecutingAssembly(), true);
                    using ITransaction transaction = session.BeginTransaction();
                    try
                    {

                        // Setup default transaction types...

                        WLabz.api.clientelle.Model.TransactionType transactionType = new() { TransactionTypeCode = Char.Parse("D"), Description = "Deposit", DRCRIndicator = WLabz.api.clientelle.Model.TransactionType.DRCRIndicators.CR };
                        await session.SaveAsync(transactionType);

                        transactionType = new() { TransactionTypeCode = Char.Parse("W"), Description = "Withdraw", DRCRIndicator = WLabz.api.clientelle.Model.TransactionType.DRCRIndicators.DR };
                        await session.SaveAsync(transactionType);

                        transactionType = new() { TransactionTypeCode = Char.Parse("A"), Description = "ATM Withdraw", DRCRIndicator = WLabz.api.clientelle.Model.TransactionType.DRCRIndicators.DR };
                        await session.SaveAsync(transactionType);

                        await transaction.CommitAsync();

                    }
                    catch
                    {
                        if (transaction != null) await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }

    }
}