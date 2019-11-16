using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RN_Process.Api.DataAccess.Entities;

namespace RN_Process.Api.Interfaces
{
    internal interface ISQLRnProcessContext
    {
       DbSet<Organization> Organizations { get; set; }
        DbSet<Term> Terms { get; set; }
        DbSet<TermDetail> TermDetails { get; set; }
        DbSet<TermDetailConfig> TermDetailConfigs { get; set; }
        DbSet<FileImport> FileImports { get; set; }

        //EntityEntry<IEntity<T>> Entry<T>(object entity);
        EntityEntry Entry(object entity);
        int SaveChanges();
    }
}