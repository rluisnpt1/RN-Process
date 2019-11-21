using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RN_Process.DataAccess.MongoDb
{
    // public interface IRepositoryNoSql<T, in TKey> where T : AuditableEntity<TKey>
    public interface IRepositoryNoSql<TEntity, in TKey> : IDisposable where TEntity
        : class, IEntity<TKey>, IAuditableEntity
    {
        Task Add(TEntity obj);
        Task<TEntity> GetById(TKey id);
        Task<IEnumerable<TEntity>> GetAll();
        Task Update(TEntity obj);
        Task Remove(TKey id);
    }
}