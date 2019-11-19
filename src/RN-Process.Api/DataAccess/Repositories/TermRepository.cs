using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.DataAccess;
using RN_Process.DataAccess.MongoDb;

namespace RN_Process.Api.DataAccess.Repositories
{
    public interface ITermRepository : IRepositoryNoSql<Term, string>
    {
        new Task SaveOneAsync(Term entity);
        new Task<bool> RemoveOneAsync(string id, bool softDelete);
    }

    public class TermRepository : BaseMongoRepository<Term, string>, ITermRepository
    {
        private readonly RnProcessMongoDbContext<Term> _repository;

        public TermRepository(IOptions<MongoDbSettings> settings) : base(settings)
        {
            _repository = new RnProcessMongoDbContext<Term>("Terms", settings);
        }

        public override async Task SaveOneAsync(Term entity)
        {
            var filter = Builders<Term>.Filter.Eq("_id", entity.Id);
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
                var update = Builders<Term>.Update
                    .Set(x => x.TermNumber, entity.TermNumber)
                    .Set(x => x.ModifiedBy, "new user need add")
                    .Set(x => x.ModifiedDate, DateTime.UtcNow)
                    .Set(x => x.RowVersion, entity.RowVersion);

                await _repository.Collection.UpdateOneAsync(filter, update);
            }
        }

        public override async Task<bool> RemoveOneAsync(string id, bool softDelete)
        {
            var filter = Builders<Term>.Filter.Eq("_id", id);
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