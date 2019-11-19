using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.DataAccess;
using RN_Process.DataAccess.MongoDb;

namespace RN_Process.Api.DataAccess.Repositories
{
    public abstract class BaseMongoRepository<T, TKey> 
        : IRepositoryNoSql<T, TKey> where T : AuditableEntity<TKey>
    {
        private readonly RnProcessMongoDbContext<T> _repository;

        protected BaseMongoRepository(IOptions<MongoDbSettings> settings)
        {
            var namePluralizing = nameof(T) + "s";

            _repository = new RnProcessMongoDbContext<T>(namePluralizing, settings);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.Collection.Find(x => true).ToListAsync();
        }

        public async Task<T> GetOneAsync(TKey id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            return await _repository.Collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetManyAsync(IEnumerable<TKey> ids)
        {
            var list = new List<T>();
            foreach (var id in ids)
            {
                var doc = await GetOneAsync(id);
                if (doc == null) continue;
                list.Add(doc);
            }

            return list;
        }

        public virtual async Task SaveOneAsync(T entity)
        {
            var filter = Builders<T>.Filter.Eq("_id", entity.Id);
            var product = _repository.Collection.Find(filter).FirstOrDefaultAsync();
            //add new
            if (product.Result == null)
            {
                entity.CreatedBy = "new user need add";
                entity.CreatedDate = DateTime.UtcNow;
                entity.ModifiedBy = string.Empty;
                entity.Deleted = false;
                entity.Active = true;
                await _repository.Collection.InsertOneAsync(entity);
            }
            else
            {
                var update = Builders<T>.Update
                    .Set(x => x.ModifiedBy, "new user need add")
                    .Set(x => x.ModifiedDate, DateTime.UtcNow)
                    .Set(x => x.RowVersion, entity.RowVersion);

                await _repository.Collection.UpdateOneAsync(filter, update);
            }
          //  await _repository.Collection.InsertOneAsync(entity);
        }

        public virtual async Task<bool> RemoveOneAsync(TKey id, bool softDelete)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            var product = _repository.Collection.Find(filter).FirstOrDefaultAsync();

            if (softDelete && product.Result != null)
            {
                product.Result.Deleted = true;
                product.Result.Active = false;
                await SaveOneAsync(product.Result);
            }
            else
            {
                await _repository.Collection.DeleteOneAsync(filter);
            }
            return true;

        }
    }
}