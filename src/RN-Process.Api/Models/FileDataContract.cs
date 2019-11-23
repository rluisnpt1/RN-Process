using System;
using System.Collections.Generic;
using System.Data;
using MongoDB.Bson;
using RN_Process.Shared.Enums;

namespace RN_Process.Api.Models
{
    public class FileDataContract
    {
        public FileDataContract()
        {
            
        }

        public FileDataContract(string id, string orgCode, string fileDescription, int fileSize, string fileFormat,
            string fileLocationOrigin, string locationToCopy, StatusType status, bool fileMigrated,
            DateTime? fileMigratedOn, List<BsonDocument> allDataInFile)
        {
            Id = id;
            OrgCode = orgCode;
            FileDescription = fileDescription;
            FileSize = fileSize;
            FileFormat = fileFormat;
            FileLocationOrigin = fileLocationOrigin;
            LocationToCopy = locationToCopy;
            Status = status;
            FileMigrated = fileMigrated;
            FileMigratedOn = fileMigratedOn;
            AllDataInFile = allDataInFile;
        }

        public string Id { get; set; }
        public string OrgCode { get; set; }
        public string FileDescription { get; set; }
        public int FileSize { get; set; }
        public string FileFormat { get; set; }
        public string FileLocationOrigin { get; set; }
        public string LocationToCopy { get; set; }

        public StatusType Status { get; set; }

        public bool FileMigrated { get; set; }
        public DateTime? FileMigratedOn { get; set; }


        public List<BsonDocument> AllDataInFile { get; set; }

    }
}