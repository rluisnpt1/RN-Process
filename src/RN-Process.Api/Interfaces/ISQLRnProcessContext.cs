﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RN_Process.Api.DataAccess.Entities;

namespace RN_Process.Api.Interfaces
{
    internal interface ISQLRnProcessContext
    {
        DbSet<ReferencesType> ReferencesTypes { get; set; }
        DbSet<Reference> References { get; set; }
        DbSet<Organization> Organizations { get; set; }
        DbSet<Contract> Contracts { get; set; }
        DbSet<ContractDetailConfig> ContractDetailConfigs { get; set; }
        DbSet<FileImport> FileImports { get; set; }

        //EntityEntry<IEntity<T>> Entry<T>(object entity);
        EntityEntry Entry(object entity);
        int SaveChanges();
    }
}