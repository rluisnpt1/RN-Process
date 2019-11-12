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
        public string FileDescription { get; set; }
        public int FileSize { get; set; }
        public string FileFormat { get; set; }
        public string FileLocationOrigin { get; set; }
        public string LocationToCopy { get; set; }

        [BsonRepresentation(BsonType.String)]
        public StatusType Status { get; set; }

        public bool FileMigrated { get; set; }
        public DateTime? FileMigratedOn { get; set; }

        public ICollection<BsonDocument> AllDataInFile { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string ContractMappingBaseId { get; set; }
        public virtual ContractMappingBase ContractMappingBase { get; set; }

        protected FileImport()
        {

        }

        public FileImport(string fileDescription, int fileSize, string fileFormat, string fileLocationOrigin,
            string locationToCopy, StatusType status, bool fileMigrated, DateTime? fileMigratedOn,
            ContractMappingBase contractMappingBase, List<BsonDocument> allDataInFile)
        {

            Id = ObjectId.GenerateNewId().ToString();

            FileDescription = fileDescription;
            FileSize = fileSize;
            FileFormat = fileFormat;
            FileLocationOrigin = fileLocationOrigin;
            LocationToCopy = locationToCopy;
            Status = status;
            FileMigrated = fileMigrated;
            FileMigratedOn = fileMigratedOn;
            AllDataInFile = allDataInFile;

            SetContractMappingBase(contractMappingBase);
        }


        private void SetContractMappingBase(ContractMappingBase contractMappingBase)
        {
            Guard.Against.Null(contractMappingBase, nameof(contractMappingBase));
            ContractMappingBaseId = contractMappingBase.ContractId;
            ContractMappingBase = contractMappingBase;
        }
    }
}
