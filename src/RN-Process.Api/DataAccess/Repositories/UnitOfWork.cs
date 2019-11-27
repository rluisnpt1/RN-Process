using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RN_Process.DataAccess.MongoDb;

namespace RN_Process.Api.DataAccess.Repositories
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
