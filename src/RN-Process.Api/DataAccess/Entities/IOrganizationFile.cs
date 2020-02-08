using System;
using System.Collections.Generic;
using MongoDB.Bson;
using RN_Process.Shared.Enums;

namespace RN_Process.Api.DataAccess.Entities
{
    public interface IOrganizationFile
    {
        string OrgCode { get; }
        string FileDescription { get; }
        int FileSize { get; }
        string FileFormat { get; }
        string FileLocationOrigin { get; }
        string LocationToCopy { get; }
        StatusType Status { get; set; }
        bool FileMigrated { get; set; }
        DateTime? FileMigratedOn { get; set; }
        List<BsonDocument> AllDataInFile { get; }
        string TermDetailConfigId { get; }
        ITermDetailConfig TermDetailConfig { get; }
        bool Deleted { get; set; }

        bool Active { get; set; }

        /// <summary>
        /// </summary>
        String Id { get; set; }
    }
}