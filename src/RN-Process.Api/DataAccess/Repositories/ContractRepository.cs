using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.DataAccess;
using RN_Process.DataAccess.MongoDb;

namespace RN_Process.Api.DataAccess.Repositories
{
    public interface IContractRepository : IRepositoryNoSql<Contract, string>
    {
        new Task SaveOneAsync(Contract entity);
        new Task<bool> RemoveOneAsync(string id, bool softDelete);
    }

    public class ContractRepository : BaseMongoRepository<Contract, string>, IContractRepository
    {
        private readonly RnProcessMongoDbContext<Contract> _repository;

        public ContractRepository(IOptions<MongoDbSettings> settings) : base(settings)
        {
            _repository = new RnProcessMongoDbContext<Contract>("Contracts", settings);
        }

        public override async Task SaveOneAsync(Contract entity)
        {
            var filter = Builders<Contract>.Filter.Eq("_id", entity.Id);
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
                var update = Builders<Contract>.Update
                    .Set(x => x.ContractNumber, entity.ContractNumber)
                    .Set(x => x.ModifiedBy, "new user need add")
                    .Set(x => x.ModifiedDate, DateTime.UtcNow)
                    .Set(x => x.RowVersion, entity.RowVersion);

                await _repository.Collection.UpdateOneAsync(filter, update);
            }
        }

        public override async Task<bool> RemoveOneAsync(string id, bool softDelete)
        {
            var filter = Builders<Contract>.Filter.Eq("_id", id);
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