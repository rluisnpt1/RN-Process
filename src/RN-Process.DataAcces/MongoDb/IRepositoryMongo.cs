using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace RN_Process.DataAccess.MongoDb
{
    public interface IRepositoryMongo<TEntity> : IDisposable where TEntity : class
    {
        TEntity Add(TEntity obj);
        Task AddAsync(TEntity obj);
        Task AddManyAsync(IEnumerable<TEntity> obj);
        Task<TEntity> GetByIdAsync(string id);
        Task<TEntity> GetByIdAsync(BsonObjectId id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetEqualField(string fieldName, string fieldValue);

        Task<TEntity> GetEntityByCodorg(string codorg);
        Task Update(TEntity obj);
        Task Remove(string id);
    }
}
