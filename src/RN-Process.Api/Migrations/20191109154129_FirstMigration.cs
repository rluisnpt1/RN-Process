﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RN_Process.Api.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 250, nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 250, nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    UniqCode = table.Column<string>(nullable: true),
                    Description = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReferencesType",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 250, nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 250, nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    UniqCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferencesType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contract",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 250, nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 250, nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    ContractNumber = table.Column<int>(nullable: false),
                    TypeDebt = table.Column<int>(nullable: false),
                    DebtDescription = table.Column<string>(nullable: true),
                    CustomerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contract_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reference",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
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
                        name: "FK_Reference_ReferencesType_ReferencesTypeId",
                        column: x => x.ReferencesTypeId,
                        principalTable: "ReferencesType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContractMappingBase",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 250, nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 250, nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CodReference = table.Column<string>(nullable: true),
                    InternalHost = table.Column<string>(nullable: true),
                    LinkToAccess = table.Column<string>(nullable: true),
                    LinkToAccesTipo = table.Column<string>(nullable: true),
                    TypeOfResponse = table.Column<string>(nullable: true),
                    RequiredLogin = table.Column<bool>(nullable: false),
                    AuthenticationLogin = table.Column<string>(nullable: true),
                    AuthenticationPassword = table.Column<string>(nullable: true),
                    AuthenticationCodeApp = table.Column<string>(nullable: true),
                    PathToOriginFile = table.Column<string>(nullable: true),
                    PathToDestinationFile = table.Column<string>(nullable: true),
                    PathToFileBackupAtClient = table.Column<string>(nullable: true),
                    PathToFileBackupAtHostServer = table.Column<string>(nullable: true),
                    FileDeLimiter = table.Column<string>(nullable: false),
                    ContractId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractMappingBase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractMappingBase_Contract_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FileImport",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 250, nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 250, nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    FileDescription = table.Column<string>(nullable: true),
                    FileSize = table.Column<int>(nullable: false),
                    FileFormat = table.Column<string>(nullable: true),
                    FileLocationOrigin = table.Column<string>(nullable: true),
                    LocationToCopy = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    FileMigrated = table.Column<bool>(nullable: false),
                    FileMigratedOn = table.Column<DateTime>(nullable: true),
                    AllDataInFile = table.Column<string>(nullable: true),
                    ContractMappingBaseId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileImport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileImport_ContractMappingBase_ContractMappingBaseId",
                        column: x => x.ContractMappingBaseId,
                        principalTable: "ContractMappingBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contract_CustomerId",
                table: "Contract",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractMappingBase_ContractId",
                table: "ContractMappingBase",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_FileImport_ContractMappingBaseId",
                table: "FileImport",
                column: "ContractMappingBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Reference_ReferencesTypeId",
                table: "Reference",
                column: "ReferencesTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileImport");

            migrationBuilder.DropTable(
                name: "Reference");

            migrationBuilder.DropTable(
                name: "ContractMappingBase");

            migrationBuilder.DropTable(
                name: "ReferencesType");

            migrationBuilder.DropTable(
                name: "Contract");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
