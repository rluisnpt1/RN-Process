using System;
using System.Threading.Tasks;

namespace RN_Process.DataAccess.MongoDb
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> Commit();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoContext _context;

        public UnitOfWork(IMongoContext context)
        {
            _context = context;
        }

        public Task<int> Commit()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}