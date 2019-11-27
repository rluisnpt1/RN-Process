using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using RN_Process.DataAccess;
using RN_Process.DataAccess.MongoDb;

namespace RN_Process.Tests
{
    public class InMemoryRepository<TEntity> : IRepositoryMongo<TEntity> where TEntity
        : class, IEntity<string>, IAuditableEntity
    {
       public InMemoryRepository()
        {
            Items = new List<TEntity>();
        }

        public List<TEntity> Items
        {
            get;
            set;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("saveThis", "Argument cannot be null.");
            }

            if (entity.Id == null)
            {
                // assign new identity value
                entity.Id = (string)(object)Convert.ChangeType(ObjectId.GenerateNewId(), typeof(string)); //GetNextIdValue();
            }

            if (Items.Contains(entity) == false)
            {
                Items.Add(entity);
            }
        }

        public async Task<TEntity> GetById(string id)
        {
            return (Items.Where(temp => temp.Id.Equals(id))).FirstOrDefault();
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return Items;
        }

        public Task Update(TEntity obj)
        {
            throw new NotImplementedException();
        }

        public Task Remove(string id)
        {
            throw new NotImplementedException();
        }
        
        public async Task Remove(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("deleteThis", "Argument cannot be null.");
            }
            if (Items.Contains(entity) == true)
            {
                Items.Remove(entity);
            }
        }
        //public async Task<IEnumerable<TEntity>> GetAllAsync()
        //{
        //    return Items;
        //}

        //public Task<IEnumerable<TEntity>> GetManyAsync(IEnumerable<TKey> ids)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<TEntity> GetOneAsync(TKey id)
        //{
        //    return (Items.Where(temp => temp.Id.Equals(id))).FirstOrDefault();
        //}

        //public Task<bool> RemoveOneAsync(TKey id, bool softDelete)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task RemoveOneAsync(TEntity entity)
        //{
        //    if (entity == null)
        //    {
        //        throw new ArgumentNullException("deleteThis", "Argument cannot be null.");
        //    }
        //    if (Items.Contains(entity) == true)
        //    {
        //        Items.Remove(entity);
        //    }
        //}

        //public async Task SaveOneAsync(TEntity entity)
        //{
        //    if (entity == null)
        //    {
        //        throw new ArgumentNullException("saveThis", "Argument cannot be null.");
        //    }

        //    if (entity.Id == null)
        //    {
        //        // assign new identity value
        //        entity.Id = (TKey)(object)Convert.ChangeType(ObjectId.GenerateNewId(), typeof(TKey)); //GetNextIdValue();
        //    }

        //    if (Items.Contains(entity) == false)
        //    {
        //        Items.Add(entity);
        //    }
        //}

    }
}
