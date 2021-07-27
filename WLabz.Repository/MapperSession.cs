//using NHibernate;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace WLabz.Repository
//{
//    public class NHibernateMapperSession : IMapperSession
//    {

//        private readonly ISession _session;
//        private ITransaction _transaction;

//        public NHibernateMapperSession(ISession session)
//        {
//            _session = session;
//        }

//        public IQueryable<IEnumerable> Items => _session.Query<T>() where T : class;

//        public void BeginTransaction()
//        {
//            _transaction = _session.BeginTransaction();
//        }

//        public void CloseTransaction()
//        {
//            if (_transaction != null)
//            {
//                _transaction.Dispose();
//                _transaction = null;
//            }
//        }

//        public async Task Commit()
//        {
//            await _transaction.CommitAsync();
//        }

//        public async Task Delete<T>(T entity) where T : class
//        {
//            await _session.DeleteAsync(entity);
//        }

//        public async Task Rollback()
//        {
//            await _transaction.RollbackAsync();
//        }

//        public async Task Save<T>(T entity) where T : class
//        {
//            await _session.SaveOrUpdateAsync(entity);
//        }
//    }
//}
