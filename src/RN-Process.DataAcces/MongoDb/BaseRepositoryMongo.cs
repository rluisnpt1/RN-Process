using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using RN_Process.DataAccess.SqlServer;

namespace RN_Process.DataAccess.MongoDb
{
    public abstract class BaseRepositoryMongo<TEntity> : IRepositoryMongo<TEntity> where TEntity : class
    {
        protected readonly IMongoContext _context;
        protected readonly IMongoCollection<TEntity> DbSet;

        protected BaseRepositoryMongo(IMongoContext context)
        {
            _context = context;
            DbSet = _context.GetCollection<TEntity>(typeof(TEntity).Name+"s");
        }

        public TEntity Add(TEntity obj)
        {
            DbSet.InsertOne(obj);
            return obj;
        }



        public virtual Task AddAsync(TEntity obj)
        {
            return _context.AddCommand(async () => await DbSet.InsertOneAsync(obj));
        }  
        
        public virtual Task AddManyAsync(IEnumerable<TEntity> obj)
        {
            return _context.AddCommand(async () => await DbSet.InsertManyAsync(obj));
        }

        public virtual async Task<TEntity> GetByIdAsync(string id)
        {
            var data = await DbSet.FindAsync(Builders<TEntity>.Filter.Eq("_id", id.Trim()));
            return data.FirstOrDefault();
        }

        public async Task<TEntity> GetEntityByCodorg(string codorg)
        {
            var filter = Builders<TEntity>.Filter.Regex("OrgCode", new BsonRegularExpression(codorg, "i"));
            return await DbSet.Find(filter).FirstOrDefaultAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var all = await DbSet.FindAsync(Builders<TEntity>.Filter.Empty);
            return all.ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> Existe(string fildValue)
        {
            var filter = Builders<TEntity>.Filter.Exists(fildValue, false);
            return await DbSet.Find(filter).ToListAsync();
        }
        public async Task<IEnumerable<TEntity>> GetEqualField(string fieldName, string fieldValue){
           

            var filter = Builders<TEntity>.Filter.Eq(fieldName, fieldValue.Trim());

            var result = await DbSet.Find(filter).ToListAsync();

            return result;
        }

        public virtual Task Update(TEntity obj)
        {
            return _context.AddCommand(async () =>
            {
                await DbSet.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", obj), obj);
            });
        }


        public virtual Task Remove(string id) => _context.AddCommand(() => DbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", id)));

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

  
}
