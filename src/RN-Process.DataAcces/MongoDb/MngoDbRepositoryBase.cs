using System;
using Microsoft.EntityFrameworkCore;

namespace RN_Process.DataAccess.MongoDb
{
    public abstract class MngoDbRepositoryBase<TEntity, TV, TDbContext> : IDisposable where TEntity : class, IEntity<TV>
        where TDbContext : DbContext
    {
        public MngoDbRepositoryBase(TDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "context is null.");
        }

        public void Dispose()
        {
            ((IDisposable)_context).Dispose();
        }

        private readonly TDbContext _context;

        protected TDbContext Context => _context;

        protected void VerifyItemIsAddedOrAttachedToDbSet(DbSet<TEntity> dbset, TEntity item)
        {
            if (item == null)
            {
                return;
            }

            if (item.Id.Equals(0) || item.Id == null)
            {
                dbset.Add(item);
            }
            else
            {
                var entry = _context.Entry<TEntity>(item);

                if (entry.State == EntityState.Detached)
                {
                    dbset.Attach(item);
                }

                entry.State = EntityState.Modified;
            }
        }
    }
}
