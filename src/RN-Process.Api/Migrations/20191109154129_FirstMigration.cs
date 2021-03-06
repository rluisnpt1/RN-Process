﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RN_Process.Api.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Organization",
                table => new
                {
                    Id = table.Column<string>(),
                    CreatedDate = table.Column<DateTime>(),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 250, nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 250, nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    UniqCode = table.Column<string>(nullable: true),
                    Description = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Organization", x => x.Id); });

            migrationBuilder.CreateTable(
                "ReferencesType",
                table => new
                {
                    Id = table.Column<string>(),
                    CreatedDate = table.Column<DateTime>(),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 250, nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 250, nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    UniqCode = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_ReferencesType", x => x.Id); });

            migrationBuilder.CreateTable(
                "Term",
                table => new
                {
                    Id = table.Column<string>(),
                    CreatedDate = table.Column<DateTime>(),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 250, nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 250, nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    TermNumber = table.Column<int>(),
                    TypeDebt = table.Column<int>(),
                    DebtDescription = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Term", x => x.Id);
                    table.ForeignKey(
                        "FK_Term_Organization_OrganizationId",
                        x => x.OrganizationId,
                        "Organization",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Reference",
                table => new
                {
                    Id = table.Column<string>(),
                    CreatedDate = table.Column<DateTime>(),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 250, nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 250, nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    UniqCode = table.Column<string>(nullable: true),
                    ReferencesTypeId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reference", x => x.Id);
                    table.ForeignKey(
                        "FK_Reference_ReferencesType_ReferencesTypeId",
                        x => x.ReferencesTypeId,
                        "ReferencesType",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "TermDetailConfig",
                table => new
                {
                    Id = table.Column<string>(),
                    CreatedDate = table.Column<DateTime>(),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 250, nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 250, nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CodReference = table.Column<string>(nullable: true),
                    InternalHost = table.Column<string>(nullable: true),
                    LinkToAccess = table.Column<string>(nullable: true),
                    LinkToAccesTipo = table.Column<string>(nullable: true),
                    TypeOfResponse = table.Column<string>(nullable: true),
                    RequiredLogin = table.Column<bool>(),
                    AuthenticationLogin = table.Column<string>(nullable: true),
                    AuthenticationPassword = table.Column<string>(nullable: true),
                    AuthenticationCodeApp = table.Column<string>(nullable: true),
                    PathToOriginFile = table.Column<string>(nullable: true),
                    PathToDestinationFile = table.Column<string>(nullable: true),
                    PathToFileBackupAtClient = table.Column<string>(nullable: true),
                    PathToFileBackupAtHostServer = table.Column<string>(nullable: true),
                    FileDeLimiter = table.Column<string>(),
                    TermId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermDetailConfig", x => x.Id);
                    table.ForeignKey(
                        "FK_TermDetailConfig_Term_TermId",
                        x => x.TermId,
                        "Term",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "FileImport",
                table => new
                {
                    Id = table.Column<string>(),
                    CreatedDate = table.Column<DateTime>(),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 250, nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 250, nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    FileDescription = table.Column<string>(nullable: true),
                    FileSize = table.Column<int>(),
                    FileFormat = table.Column<string>(nullable: true),
                    FileLocationOrigin = table.Column<string>(nullable: true),
                    LocationToCopy = table.Column<string>(nullable: true),
                    Status = table.Column<int>(),
                    FileMigrated = table.Column<bool>(),
                    FileMigratedOn = table.Column<DateTime>(nullable: true),
                    AllDataInFile = table.Column<string>(nullable: true),
                    TermDetailConfigId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileImport", x => x.Id);
                    table.ForeignKey(
                        "FK_FileImport_TermDetailConfig_TermDetailConfigId",
                        x => x.TermDetailConfigId,
                        "TermDetailConfig",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_Term_OrganizationId",
                "Term",
                "OrganizationId");

            migrationBuilder.CreateIndex(
                "IX_TermDetailConfig_TermId",
                "TermDetailConfig",
                "TermId");

            migrationBuilder.CreateIndex(
                "IX_FileImport_TermDetailConfigId",
                "FileImport",
                "TermDetailConfigId");

            migrationBuilder.CreateIndex(
                "IX_Reference_ReferencesTypeId",
                "Reference",
                "ReferencesTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "FileImport");

            migrationBuilder.DropTable(
                "Reference");

            migrationBuilder.DropTable(
                "TermDetailConfig");

            migrationBuilder.DropTable(
                "ReferencesType");

            migrationBuilder.DropTable(
                "Term");

            migrationBuilder.DropTable(
                "Organization");
        }
    }
}