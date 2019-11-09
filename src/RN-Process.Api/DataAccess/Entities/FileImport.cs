using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RN_Process.DataAccess;
using RN_Process.Shared.Enums;

namespace RN_Process.Api.DataAccess.Entities
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

        public string AllDataInFile { get; set; }
        //  public virtual ICollection<AllDataInFile> AllDataInFile { get; set; }

        //runtime execution
        protected FileImport()
        {
           
        }

        public FileImport(ContractMappingBase contractMappingBase)
        {
         
            ContractMappingBase = contractMappingBase;
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string ContractMappingBaseId { get; set; }
        public virtual ContractMappingBase ContractMappingBase { get; set; }
    }
}
