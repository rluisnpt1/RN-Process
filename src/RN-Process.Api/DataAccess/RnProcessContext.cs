using System.Linq;
using Microsoft.EntityFrameworkCore;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Api.Interfaces;

namespace RN_Process.Api.DataAccess
{
    public class RnProcessContext : DbContext, IRnProcessContext
    {
        public RnProcessContext(DbContextOptions options) :
            base(options)
        {

        }
        public DbSet<ReferencesType> ReferencesTypes { get; set; }
        public DbSet<Reference> References { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ContractMappingBase> ContractMappingBases { get; set; }
        public DbSet<FileImport> FileImports { get; set; }

     

        public override int SaveChanges()
        {
            CleanupOrphanedPersonFacts();

            return base.SaveChanges();
        }

        private void CleanupOrphanedPersonFacts()
        {
            var deleteThese = Contracts.Local.Where(pf => pf.Customer == null).ToList();
            foreach (var deleteThis in deleteThese)
            {
                Contracts.Remove(deleteThis);
            }
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
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");
                entity.HasKey(x => x.Id);

                entity.HasMany(x => x.Contracts)
                    .WithOne(x => x.Customer);

                entity.Property(x => x.UniqCode).IsUnicode();
                entity.Property(x => x.RowVersion).IsConcurrencyToken();
            });

            modelBuilder.Entity<Contract>(entity =>
            {
                entity.ToTable("Contract");

                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.ContractMappingBases)
                    .WithOne(x => x.Contract);

                entity.Property(x => x.RowVersion).IsConcurrencyToken();
            });

            modelBuilder.Entity<ContractMappingBase>(entity =>
            {
                entity.ToTable("ContractMappingBase");
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.FileImports).WithOne(x => x.ContractMappingBase);
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
