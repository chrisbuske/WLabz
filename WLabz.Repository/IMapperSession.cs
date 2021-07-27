using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

namespace WLabz.Repository
{
    public interface IMapperSession
    {

        void BeginTransaction();
        Task Commit();
        Task Rollback();
        void CloseTransaction();
        Task Save<T>(T entity)
            where T : class;
        Task Delete<T>(T entity)
           where T : class;
        //IQueryable Items<T> { get; } where T : class;

    }
}