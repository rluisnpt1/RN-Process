﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RN_Process.Api.DataAccess;

namespace RN_Process.Api.Migrations
{
    [DbContext(typeof(RnProcessContext))]
    [Migration("20191109154129_FirstMigration")]
    partial class FirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RN_Process.Api.DataAccess.Entities.Contract", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ContractNumber")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("OrganizationId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DebtDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<int>("TypeDebt")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Contract");
                });

            modelBuilder.Entity("RN_Process.Api.DataAccess.Entities.ContractMappingBase", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AuthenticationCodeApp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AuthenticationLogin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AuthenticationPassword")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CodReference")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContractId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FileDeLimiter")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("InternalHost")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LinkToAccesTipo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LinkToAccess")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PathToDestinationFile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PathToFileBackupAtClient")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PathToFileBackupAtHostServer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PathToOriginFile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("RequiredLogin")
                        .HasColumnType("bit");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("TypeOfResponse")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ContractId");

                    b.ToTable("ContractMappingBase");
                });

            modelBuilder.Entity("RN_Process.Api.DataAccess.Entities.Organization", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("UniqCode")
                        .HasColumnType("nvarchar(max)")
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.ToTable("Organization");
                });

            modelBuilder.Entity("RN_Process.Api.DataAccess.Entities.FileImport", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AllDataInFile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContractMappingBaseId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FileDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileFormat")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileLocationOrigin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("FileMigrated")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("FileMigratedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("FileSize")
                        .HasColumnType("int");

                    b.Property<string>("LocationToCopy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ContractMappingBaseId");

                    b.ToTable("FileImport");
                });

            modelBuilder.Entity("RN_Process.Api.DataAccess.Entities.Reference", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ReferencesTypeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("UniqCode")
                        .HasColumnType("nvarchar(max)")
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.HasIndex("ReferencesTypeId");

                    b.ToTable("Reference");
                });

            modelBuilder.Entity("RN_Process.Api.DataAccess.Entities.ReferencesType", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("UniqCode")
                        .HasColumnType("nvarchar(max)")
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.ToTable("ReferencesType");
                });

            modelBuilder.Entity("RN_Process.Api.DataAccess.Entities.Contract", b =>
                {
                    b.HasOne("RN_Process.Api.DataAccess.Entities.Organization", "Organization")
                        .WithMany("Contracts")
                        .HasForeignKey("OrganizationId");
                });

            modelBuilder.Entity("RN_Process.Api.DataAccess.Entities.ContractMappingBase", b =>
                {
                    b.HasOne("RN_Process.Api.DataAccess.Entities.Contract", "Contract")
                        .WithMany("ContractMappingBases")
                        .HasForeignKey("ContractId");
                });

            modelBuilder.Entity("RN_Process.Api.DataAccess.Entities.FileImport", b =>
                {
                    b.HasOne("RN_Process.Api.DataAccess.Entities.ContractMappingBase", "ContractMappingBase")
                        .WithMany("FileImports")
                        .HasForeignKey("ContractMappingBaseId");
                });

            modelBuilder.Entity("RN_Process.Api.DataAccess.Entities.Reference", b =>
                {
                    b.HasOne("RN_Process.Api.DataAccess.Entities.ReferencesType", "ReferencesType")
                        .WithMany("References")
                        .HasForeignKey("ReferencesTypeId");
                });
#pragma warning restore 612, 618
        }
    }
}
