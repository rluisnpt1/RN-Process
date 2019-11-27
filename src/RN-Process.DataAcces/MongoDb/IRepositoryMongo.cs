using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RN_Process.DataAccess.MongoDb
{
    public interface IRepositoryMongo<TEntity> : IDisposable where TEntity : class
    {
        Task Add(TEntity obj);
        Task<TEntity> GetById(string id);
        Task<IEnumerable<TEntity>> GetAll();
        Task Update(TEntity obj);
        Task Remove(string id);
    }
}
