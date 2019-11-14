using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RN_Process.DataAccess;
using RN_Process.Shared.Commun;
using RN_Process.Shared.Enums;

namespace RN_Process.Api.DataAccess.Entities
{
    public class FileImport : AuditableEntity<string>
    {
        protected FileImport()
        {
        }

        public FileImport(string fileDescription, int fileSize, string fileFormat, string fileLocationOrigin,
            string locationToCopy, StatusType status, bool fileMigrated, DateTime? fileMigratedOn,
            ContractDetailConfig contractDetailConfig, BsonDocument allDataInFile)
        {
            Id = ObjectId.GenerateNewId().ToString();

            SetFileDescritption(fileDescription);
            SetFileSize(fileSize);
            SetFileFormat(fileFormat);
            FileLocationOrigin = fileLocationOrigin;
            LocationToCopy = locationToCopy;
            Status = status;
            SetMigration(fileMigrated);
            SetMigrationDate(fileMigratedOn);
            SetAllDatafromFile(allDataInFile);
            Active = true;
            Deleted = false;

            SetContractDetailConfig(contractDetailConfig);
        }
        public virtual string OrgCode { get; private set; }
        public string FileDescription { get; set; }
        public int FileSize { get; set; }
        public string FileFormat { get; set; }
        public string FileLocationOrigin { get; set; }
        public string LocationToCopy { get; set; }

        [BsonRepresentation(BsonType.String)] public StatusType Status { get; set; }

        public bool FileMigrated { get; set; }
        public DateTime? FileMigratedOn { get; set; }


        public BsonDocument AllDataInFile { get; set; }


        [BsonRepresentation(BsonType.ObjectId)]
        public string ContractDetailConfigId { get; set; }

        public virtual ContractDetailConfig ContractDetailConfig { get; set; }

        private void SetAllDatafromFile(BsonDocument allDataInFile)
        {
            Guard.Against.Null(allDataInFile, nameof(allDataInFile));
           
            AllDataInFile = allDataInFile;
        }

        private void SetMigrationDate(DateTime? fileMigratedOn)
        {
            if (fileMigratedOn != null)
                FileMigratedOn = fileMigratedOn.Value.Date;
            else
                FileMigratedOn = null;
        }

        private void SetMigration(bool fileMigrated)
        {
            FileMigrated = fileMigrated;
        }

        private void SetFileFormat(string fileFormat)
        {
            Guard.Against.NullOrEmpty(fileFormat, nameof(fileFormat));
            Guard.Against.NullOrWhiteSpace(fileFormat, nameof(fileFormat));
            FileFormat = fileFormat;
        }

        private void SetFileSize(int fileSize)
        {
            Guard.Against.Zero(fileSize, nameof(fileSize));
            FileSize = fileSize;
        }

        private void SetFileDescritption(string fileDescription)
        {
            Guard.Against.NullOrEmpty(fileDescription, nameof(fileDescription));
            Guard.Against.NullOrWhiteSpace(fileDescription, nameof(fileDescription));
            FileDescription = fileDescription;
        }


        private void SetContractDetailConfig(ContractDetailConfig contractDetailConfig)
        {
            Guard.Against.Null(contractDetailConfig, nameof(contractDetailConfig));
            ContractDetailConfigId = contractDetailConfig.ContractId;
            OrgCode = contractDetailConfig.OrgCode;
            ContractDetailConfig = contractDetailConfig;
        }
    }
}