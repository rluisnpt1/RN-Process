using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace RN_Process.Api.Models
{
    public class FileDataContract
    {
        public FileDataContract()
        {
            AllDataInFile = new List<BsonDocument>();
        }

        public FileDataContract(string id, string orgCode, string fileDescription, int fileSize, string fileFormat,
            string fileLocationOrigin, string locationToCopy, string status, bool fileMigrated,
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
            FileMigrated = fileMigrated.ToString();
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

       // [JsonConverter(typeof(StringEnumConverter))]
        public string Status { get; set; }

        public string FileMigrated { get; set; }
        public DateTime? FileMigratedOn { get; set; }


        public List<BsonDocument> AllDataInFile { get; set; }

    }
}