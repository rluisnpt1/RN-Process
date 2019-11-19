using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using RN_Process.DataAccess;

namespace RN_Process.Tests
{
    public class InMemoryRepository<T, TKey> : IRepositoryNoSql<T, TKey> where T : AuditableEntity<TKey>
    {
       public InMemoryRepository()
        {
            Items = new List<T>();
        }

        public List<T> Items
        {
            get;
            set;
        }


        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return Items;
        }

        public Task<IEnumerable<T>> GetManyAsync(IEnumerable<TKey> ids)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetOneAsync(TKey id)
        {
            return (Items.Where(temp => temp.Id.Equals(id))).FirstOrDefault();
        }

        public Task<bool> RemoveOneAsync(TKey id, bool softDelete)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveOneAsync(T entity)
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

        public async Task SaveOneAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("saveThis", "Argument cannot be null.");
            }

            if (entity.Id == null)
            {
                // assign new identity value
                entity.Id = (TKey)(object)Convert.ChangeType(ObjectId.GenerateNewId(), typeof(TKey)); //GetNextIdValue();
            }

            if (Items.Contains(entity) == false)
            {
                Items.Add(entity);
            }
        }
    }
}
