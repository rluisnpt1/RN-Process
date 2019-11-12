using System;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Api.DataAccess.Persistences;
using RN_Process.DataAccess.MongoDb;
using RN_Process.Shared.Commun;

namespace RN_Process.Api.DataAccess
{
    public class RnProcessMongoDbContext<T> : IRnProcessMongoDbContext<T>
    {
        public IMongoDatabase Database { get; }
        public IMongoCollection<T> Collection { get; }

        public RnProcessMongoDbContext(string collectionName, IOptions<MongoDbSettings> settings)
        {
            Guard.Against.NullOrEmpty(collectionName,nameof(collectionName));
            Guard.Against.Null(settings, nameof(settings));
          
            try
            {
                var client = new MongoClient(settings.Value.ConnectionString);
                Database = client.GetDatabase(settings.Value.Database);
                Collection = Database.GetCollection<T>(collectionName);
                //_database.GetCollection<Product>("Products");
            }
            catch (Exception ex)
            {
                throw new Exception("Can not access to MongoDb server.", ex);
            }
     

            RegisterMapIfNeeded<Organization>();
            RegisterMapIfNeeded<Contract>();
            RegisterMapIfNeeded<ContractDetailConfig>();
            RegisterMapIfNeeded<FileImport>();
        }


        // Check to see if map is registered before registering class map
        // This is for the sake of the polymorphic types that we are using so Mongo knows how to deserialize
        private void RegisterMapIfNeeded<TClass>()
        {
            //MongoDbMap.OrganizationConfigure();
            //MongoDbMap.ContractConfigure();
            //MongoDbMap.ContractDetailConfigConfigure();
            //MongoDbMap.FileImPortConfigure();

            if (!BsonClassMap.IsClassMapRegistered(typeof(TClass)))
                BsonClassMap.RegisterClassMap<TClass>();

            // Set Guid to CSharp style (with dash -)
            BsonDefaults.GuidRepresentation = GuidRepresentation.CSharpLegacy;
            // Conventions
            var pack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true),
                new IgnoreIfDefaultConvention(true)
            };
            ConventionRegistry.Register("RnProcess Solution Conventions", pack, t => true);
        }
    }

    public interface IRnProcessMongoDbContext<T> 
    {
        IMongoDatabase Database { get; }
        IMongoCollection<T> Collection { get; }
    }
}
