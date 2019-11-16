using System.Linq;
using Microsoft.EntityFrameworkCore;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Api.Interfaces;

namespace RN_Process.Api.DataAccess
{
    public class RnProcessContext : DbContext, ISQLRnProcessContext
    {
        public RnProcessContext(DbContextOptions options) :
            base(options)
        {
        }

        public DbSet<ReferencesType> ReferencesTypes { get; set; }
        public DbSet<Reference> References { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Term> Terms { get; set; }
        public DbSet<TermDetailConfig> TermDetailConfigs { get; set; }
        public DbSet<FileImport> FileImports { get; set; }


        public override int SaveChanges()
        {
            CleanupOrphanedPersonFacts();

            return base.SaveChanges();
        }

        private void CleanupOrphanedPersonFacts()
        {
            var deleteThese = Terms.Local.Where(pf => pf.Organization == null).ToList();
            foreach (var deleteThis in deleteThese) Terms.Remove(deleteThis);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReferencesType>(entity =>
            {
                entity.ToTable("ReferencesType");
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.References)
                    .WithOne(x => x.ReferencesType);

                entity.Property(x => x.UniqCode).IsUnicode();
                entity.Property(x => x.RowVersion).IsConcurrencyToken();
            });
            modelBuilder.Entity<Reference>(entity =>
            {
                entity.ToTable("Reference");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.UniqCode).IsUnicode();
                entity.Property(x => x.RowVersion).IsConcurrencyToken();
            });
            modelBuilder.Entity<Organization>(entity =>
            {
                entity.ToTable("Organization");
                entity.HasKey(x => x.Id);

                entity.HasMany(x => x.Terms)
                    .WithOne(x => x.Organization);

                entity.Property(x => x.OrgCode).IsUnicode();
                entity.Property(x => x.RowVersion).IsConcurrencyToken();
            });

            modelBuilder.Entity<Term>(entity =>
            {
                entity.ToTable("Term");

                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.TermDetails)
                    .WithOne(x => x.Term);

                entity.Property(x => x.RowVersion).IsConcurrencyToken();
            });

            modelBuilder.Entity<TermDetailConfig>(entity =>
            {
                entity.ToTable("TermDetailConfig");
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.FileImports).WithOne(x => x.TermDetailConfig);
                entity.Property(x => x.RowVersion).IsConcurrencyToken();
            });
            modelBuilder.Entity<FileImport>(entity =>
            {
                entity.ToTable("FileImport");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.RowVersion).IsConcurrencyToken();
            });
        }
    }
}