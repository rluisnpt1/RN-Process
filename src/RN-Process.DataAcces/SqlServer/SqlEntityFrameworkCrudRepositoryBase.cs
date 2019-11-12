using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace RN_Process.DataAccess.SqlServer
{
    public abstract class MongoDbCrudRepositoryBase<TEntity, V, TDbContext> :
        MngodbRepositoryBase<TEntity, V, TDbContext>, IRepository<TEntity, V>
        where TEntity : class, IEntity<V>
        where TDbContext : DbContext
    {
        public MongoDbCrudRepositoryBase(TDbContext context) : base(context)
        {
        }

        protected abstract DbSet<TEntity> EntityDbSet { get; }

        public virtual void Delete(TEntity deleteThis)
        {
            if (deleteThis == null)
                throw new ArgumentNullException("deleteThis", "deleteThis is null.");

            var entry = Context.Entry(deleteThis);

            if (entry.State == EntityState.Detached) EntityDbSet.Attach(deleteThis);

            EntityDbSet.Remove(deleteThis);

            Context.SaveChanges();
        }

        public virtual IList<TEntity> GetAll()
        {
            return EntityDbSet.ToList();
        }


        public TEntity GetById(V id)
        {
            return EntityDbSet.FirstOrDefault(x => x.Id.Equals(id));
        }

        public virtual void Save(TEntity saveThis)
        {
            if (saveThis == null)
                throw new ArgumentNullException("saveThis", "saveThis is null.");

            VerifyItemIsAddedOrAttachedToDbSet(
                EntityDbSet, saveThis);

            Context.SaveChanges();
        }
    }
}