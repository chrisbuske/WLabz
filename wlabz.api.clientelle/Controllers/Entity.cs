using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using WLabz.Logs;
using WLabz.Repository;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WLabz.api.clientelle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Entity : ControllerBase
    {

        #region Declarations...

        private readonly IRabbitLogger _logger;
        private readonly IFluentHibernateSessionManager _persistance;

        #endregion

        #region Constructors...

        public Entity(IRabbitLogger logger, IFluentHibernateSessionManager persistance)
        {
            _logger = logger;
            _persistance = persistance;
        }

        #endregion

        /// <summary>
        /// Retrieve an entity...
        /// </summary>
        /// <param name="id">Unique Entity Identifier</param>
        /// <returns>Return the entity information</returns>
        // GET api/<Entity>/00000000-0000-0000-0000-000000000000
        [HttpGet("{id}")]
        public async Task<Model.Entity> Get(Guid id)
        {

            using ISession session = _persistance.OpenSession(System.Reflection.Assembly.GetExecutingAssembly(), false);
            using ITransaction transaction = session.BeginTransaction();
            try
            {

                // Retrieve the entity...

                Model.Entity entity = await session.GetAsync<Model.Entity>(id);

                await transaction.CommitAsync();

                return entity;

            }
            catch
            {
                if (transaction != null) await transaction.RollbackAsync();
                throw;
            }

        }

        /// <summary>
        /// Create a new entity...
        /// </summary>
        /// <param name="entity">Entity information to be saved</param>
        /// <returns>Recorded Entity</returns>
        [HttpPost]
        public async Task<Model.Entity> Post(Model.Entity entity)
        {

            using ISession session = _persistance.OpenSession(System.Reflection.Assembly.GetExecutingAssembly(), false);
            using ITransaction transaction = session.BeginTransaction();
            try
            {

                // Remove any accounts...

                if (entity.Accounts != null) entity.Accounts.Clear();

                // Ensure the entity contains either an individuals information or and organisations information...

                if (entity.Individual == null && entity.Organisation == null)
                    throw new Exception("Require the entity information for an individual or organisation");
                else if (entity.EntityType == Model.Entity.EntityTypes.Individual && entity.Individual == null)
                    throw new Exception("Enitiy specified as individual but is missing the individual's information");
                else if (entity.EntityType == Model.Entity.EntityTypes.Organisation && entity.Organisation == null)
                    throw new Exception("Enitiy specified as organisation but is missing the organisation's information");

                // Save the entity information...
                
                await session.SaveAsync(entity);

                if (entity.EntityType == Model.Entity.EntityTypes.Individual)
                {
                    entity.Individual.EntityID = entity.EntityID;
                    await session.SaveAsync(entity.Individual);
                }
                else if (entity.EntityType == Model.Entity.EntityTypes.Organisation)
                {
                    entity.Organisation.EntityID = entity.EntityID;
                    await session.SaveAsync(entity.Organisation);
                }

                await transaction.CommitAsync();

                _logger.PublishTransactionHistory(Guid.NewGuid(), TransactionHistory.EActions.Post, DateTime.Now, entity);

                return entity;

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
        /// <param name="entity">Entity inormation to be updated</param>
        /// <returns>Updated Entity</returns>
        [HttpPut]
        public async Task<Model.Entity> Put(Model.Entity entity)
        {
            using ISession session = _persistance.OpenSession(System.Reflection.Assembly.GetExecutingAssembly(), false);
            using ITransaction transaction = session.BeginTransaction();
            try
            {
                // Remove any accounts...

                entity.Accounts.Clear();

                // Ensure the entity contains either an individuals information or and organisations information...

                if (entity.Individual == null && entity.Organisation == null)
                    throw new Exception("Require the entity information for an individual or organisation");
                else if (entity.EntityType == Model.Entity.EntityTypes.Individual && entity.Individual == null)
                    throw new Exception("Enitiy specified as individual but is missing the individual's information");
                else if (entity.EntityType == Model.Entity.EntityTypes.Organisation && entity.Organisation == null)
                    throw new Exception("Enitiy specified as organisation but is missing the organisation's information");

                // Update the entity information...

                if (entity.EntityType == Model.Entity.EntityTypes.Individual)
                {
                    entity.Individual.EntityID = entity.EntityID;
                    await session.UpdateAsync(entity.Individual);
                }
                else if (entity.EntityType == Model.Entity.EntityTypes.Organisation)
                {
                    entity.Organisation.EntityID = entity.EntityID;
                    await session.UpdateAsync(entity.Organisation);
                }

                await transaction.CommitAsync();

                _logger.PublishTransactionHistory(Guid.NewGuid(), TransactionHistory.EActions.Put, DateTime.Now, entity);

                return entity;

            }
            catch
            {
                if (transaction != null) await transaction.RollbackAsync();
                throw;
            }
        }

    }
}
