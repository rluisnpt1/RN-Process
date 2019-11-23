using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.DataAccess.MongoDb;

namespace RN_Process.Api.DataAccess
{
    public class RnProcessMongoDbContext : IMongoContext
    {
        private readonly List<Func<Task>> _commands;

        public RnProcessMongoDbContext(IConfiguration configuration)
        {
            // Set Guid to CSharp style (with dash -)
            BsonDefaults.GuidRepresentation = GuidRepresentation.CSharpLegacy;

            // Every command will be stored and it'll be processed at SaveChanges
            _commands = new List<Func<Task>>();

            RegisterConventions<Organization>();
            RegisterConventions<Term>();
            RegisterConventions<TermDetail>();
            RegisterConventions<TermDetailConfig>();
            RegisterConventions<OrganizationFile>();

            // Configure mongo (You can inject the config, just to simplify)
            var connection = Environment.GetEnvironmentVariable("MONGOCONNECTION") ??
                             configuration.GetSection("MongoConnection").GetSection("ConnectionString").Value;
            var db = Environment.GetEnvironmentVariable("DATABASENAME") ??
                     configuration.GetSection("MongoConnection").GetSection("Database")
                         .Value;
            MongoClient = new MongoClient(connection);

            Database = MongoClient.GetDatabase(db);
        }

        private IMongoDatabase Database { get; }
        private MongoClient MongoClient { get; }
        private IClientSessionHandle Session { get; set; }

        public async Task<int> SaveChanges()
        {
            using (Session = await MongoClient.StartSessionAsync())
            {
                Session.StartTransaction();

                var commandTasks = _commands.Select(c => c());

                await Task.WhenAll(commandTasks);

                await Session.CommitTransactionAsync();
            }

            return _commands.Count;
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return Database.GetCollection<T>(name);
        }

        public void Dispose()
        {
            while (Session != null && Session.IsInTransaction)
                Thread.Sleep(TimeSpan.FromMilliseconds(100));

            GC.SuppressFinalize(this);
        }

        public virtual async Task AddCommand(Func<Task> func)
        {
            _commands.Add(func);
        }

        private void RegisterConventions<TClass>()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(TClass)))
                BsonClassMap.RegisterClassMap<TClass>();

            // Set Guid to CSharp style (with dash -)
            BsonDefaults.GuidRepresentation = GuidRepresentation.CSharpLegacy;
            var pack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true),
                new IgnoreIfDefaultConvention(true)
            };
            ConventionRegistry.Register("RnFileProcess Conventions", pack, t => true);
        }
    }
}