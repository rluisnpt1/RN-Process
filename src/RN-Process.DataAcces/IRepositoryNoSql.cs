using System.Collections.Generic;
using System.Threading.Tasks;

namespace RN_Process.DataAccess
{
    public interface IRepositoryNoSql<T, in TKey> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();

        //Task<T> GetOneAsync(T entity);
        Task<T> GetOneAsync(TKey id);

        //Task<T> GetManyAsync(IEnumerable<T> entity);
        Task<IEnumerable<T>> GetManyAsync(IEnumerable<TKey> ids);

        //  Task<T> SaveOneAsync(T entity);
        Task SaveOneAsync(T entity);

        ////   Task<T> SaveManyAsync(IEnumerable<T> entity);
        //Task<bool> UpdateAsync(T entity);
        Task<bool> RemoveOneAsync(TKey id, bool softDelete);
        //Task<bool> RemoveManyAsync(IEnumerable<TKey> ids);
    }
}