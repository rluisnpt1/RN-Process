using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RN_Process.DataAccess;
using RN_Process.Shared.Commun;
using RN_Process.Shared.Enums;

namespace RN_Process.Api.DataAccess.Entities
{
    public class OrganizationFile : AuditableEntity<string>, IOrganizationFile
    {
        protected OrganizationFile()
        {
        }

        public OrganizationFile(string id, string fileDescription, int fileSize, string fileFormat,
            string fileLocationOrigin,
            string locationToCopy, StatusType status, bool fileMigrated, DateTime? fileMigratedOn,
            TermDetailConfig termDetailConfig, List<BsonDocument> allDataInFile)
        {
            Id = string.IsNullOrWhiteSpace(id) ? ObjectId.GenerateNewId().ToString() : id;

            SetFileDescritption(fileDescription);
            SetFileSize(fileSize);
            SetFileFormat(fileFormat);
            FileLocationOrigin = fileLocationOrigin;
            LocationToCopy = locationToCopy;
            
            SetMigration(fileMigrated);
            SetMigrationDate(fileMigratedOn);
            SetAllDatafromFile(allDataInFile);
            Deleted = false;
            Active = true;
            SetStatus(status);

            SetTermDetailConfig(termDetailConfig);
        }

        public virtual string OrgCode { get; private set; }
        public string FileDescription { get; private set; }
        public int FileSize { get; private set; }
        public string FileFormat { get; private set; }
        public string FileLocationOrigin { get; }
        public string LocationToCopy { get; }

        [BsonRepresentation(BsonType.String)] public StatusType Status { get; set; }

        public bool FileMigrated { get; set; }
        public DateTime? FileMigratedOn { get; set; }


        public List<BsonDocument> AllDataInFile { get; private set; }


        [BsonRepresentation(BsonType.ObjectId)]
        public string TermDetailConfigId { get; private set; }

        public virtual ITermDetailConfig TermDetailConfig { get; private set; }

        private void SetStatus(StatusType status)
        {
            if (status != StatusType.Success || status != StatusType.Processed)
                Active = false;
            else
                Active = true;
            Status = status;
        }

        private void SetAllDatafromFile(List<BsonDocument> allDataInFile)
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


        private void SetTermDetailConfig(TermDetailConfig termDetailConfig)
        {
            Guard.Against.Null(termDetailConfig, nameof(termDetailConfig));
            TermDetailConfigId = termDetailConfig.Id;
            OrgCode = termDetailConfig.OrgCode;
            TermDetailConfig = termDetailConfig;
        }
    }
}