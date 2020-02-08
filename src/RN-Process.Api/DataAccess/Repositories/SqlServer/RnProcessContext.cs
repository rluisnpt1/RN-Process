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

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Term> Terms { get; set; }
        public DbSet<TermDetail> TermDetails { get; set; }
        public DbSet<TermDetailConfig> TermDetailConfigs { get; set; }
        public DbSet<OrganizationFile> FileImports { get; set; }


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
            modelBuilder.Entity<Organization>(entity =>
            {
                entity.ToTable("Organization");
                entity.HasKey(x => x.Id);

                entity.HasMany(x => x.Terms)
                    .WithOne(x => (Organization) x.Organization);

                entity.Property(x => x.OrgCode).IsUnicode();
                entity.Property(x => x.RowVersion).IsConcurrencyToken();
            });

            modelBuilder.Entity<Term>(entity =>
            {
                entity.ToTable("Term");

                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.TermDetails)
                    .WithOne(x => (Term) x.Term);

                entity.Property(x => x.RowVersion).IsConcurrencyToken();
            });

            modelBuilder.Entity<TermDetail>(entity =>
            {
                entity.ToTable("TermDetail");

                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.TermDetailConfigs)
                    .WithOne(x => x.TermDetail as TermDetail);

                entity.Property(x => x.RowVersion).IsConcurrencyToken();
            });

            modelBuilder.Entity<TermDetailConfig>(entity =>
            {
                entity.ToTable("TermDetailConfig");
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.OrganizationFiles).WithOne(x => (TermDetailConfig) x.TermDetailConfig);
                entity.Property(x => x.RowVersion).IsConcurrencyToken();
            });
            modelBuilder.Entity<OrganizationFile>(entity =>
            {
                entity.ToTable("OrganizationFile");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.RowVersion).IsConcurrencyToken();
            });
        }
    }
}