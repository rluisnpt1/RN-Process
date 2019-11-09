
using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using RN_Process.DataAccess;
using RN_Process.Shared.Enums;

namespace RN_Process.Api.Models
{
    public class FileImport : Entity<string>
    {
        public string FileDescription { get; set; }
        public int FileSize { get; set; }
        public string FileFormat { get; set; }
        public string FileLocationOrigin { get; set; }
        public string LocationToCopy { get; set; }
        public StatusType Status { get; set; }
        public bool FileMigrated { get; set; }
        public DateTime? FileMigratedOn { get; set; }

        public virtual List<BsonDocument> AllDataInFile { get; set; }

        public FileImport(ContractMappingBase contractMappingBase)
        {
            ContractMappingBase = contractMappingBase;
        }

        public string ContractMappingBaseId { get; set; }
        public virtual ContractMappingBase ContractMappingBase { get; set; }
    }
}
