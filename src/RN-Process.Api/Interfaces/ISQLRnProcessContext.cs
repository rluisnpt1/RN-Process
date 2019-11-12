using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.DataAccess;

namespace RN_Process.Api.Interfaces
{
    interface ISQLRnProcessContext
    {

        DbSet<ReferencesType> ReferencesTypes { get; set; }
        DbSet<Reference> References { get; set; }
        DbSet<Organization> Organizations { get; set; }
        DbSet<Contract> Contracts { get; set; }
        DbSet<ContractMappingBase> ContractMappingBases { get; set; }
        DbSet<FileImport> FileImports { get; set; }

        //EntityEntry<IEntity<T>> Entry<T>(object entity);
        EntityEntry Entry(object entity);
        int SaveChanges();
    }

}
