using System;
using Microsoft.EntityFrameworkCore;

namespace RN_Process.DataAccess.SqlServer
{
    public abstract class MngodbRepositoryBase<TEntity, TV, TDbContext> : IDisposable where TEntity : class, IEntity<TV>
        where TDbContext : DbContext
    {
        public MngodbRepositoryBase(TDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context), "context is null.");
        }

        protected TDbContext Context { get; }

        public void Dispose()
        {
            ((IDisposable) Context).Dispose();
        }

        protected void VerifyItemIsAddedOrAttachedToDbSet(DbSet<TEntity> dbset, TEntity item)
        {
            if (item == null) return;

            if (item.Id.Equals(0) || item.Id == null)
            {
                dbset.Add(item);
            }
            else
            {
                var entry = Context.Entry(item);

                if (entry.State == EntityState.Detached) dbset.Attach(item);

                entry.State = EntityState.Modified;
            }
        }
    }
}