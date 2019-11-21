using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace RN_Process.DataAccess.MongoDb
{
    public interface IMongoContext : IDisposable
    {
        Task AddCommand(Func<Task> func);
        Task<int> SaveChanges();
        IMongoCollection<T> GetCollection<T>(string name);
    }
}