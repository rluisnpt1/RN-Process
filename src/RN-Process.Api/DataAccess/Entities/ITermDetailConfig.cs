using System;
using System.Collections.Generic;
using MongoDB.Bson;
using RN_Process.Shared.Enums;

namespace RN_Process.Api.DataAccess.Entities
{
    public interface ITermDetailConfig
    {
        int DirectoryHostServerSize { get; set; }
        string Id { get; }
        string TermDetailId { get; }
        ITermDetail TermDetail { get; set; }
        string OrgCode { get; }
        FileAccessType CommunicationType { get; }
        string InternalHost { get; }
        string BaseWorkDirectoryHost { get; }
        string LinkToAccess { get; }
        string LinkToAccessType { get; }
        string TypeOfResponse { get; }
        bool RequiredLogin { get; }
        string AuthenticationLogin { get; }
        byte[] AuthenticationPassword { get; }
        byte[] HostKeyFingerPrint { get; }
        string AuthenticationCodeApp { get; }
        string PathToOriginFile { get; }
        string PathToDestinationFile { get; }
        string PathToFileBackupAtClient { get; }
        string PathToFileBackupAtHostServer { get; }
        string FileDelimiter { get; }
        string FileName { get; }
        bool HasHeader { get; }
        string FileProtectedPassword { get; }
        IList<string> FileHeaderColumns { get; }
        IList<string> AvailableFieldsColumns { get; }
        ICollection<IOrganizationFile> OrganizationFiles { get; }
        DateTime CreatedDate { get; set; }
        string CreatedBy { get; set; }
        DateTime? UpdatedDate { get; set; }
        string ModifiedBy { get; set; }
        bool Deleted { get; set; }
        bool Active { get; set; }
        byte[] RowVersion { get; set; }

        void AddOrganizationFile(string id, string orgCode, string fileDescription, int fileSize,
            string fileFormat,
            string fileLocationOrigin, string locationToCopy, StatusType status, bool fileMigrated,
            DateTime? fileMigratedOn, List<BsonDocument> allDataInFile, bool active);

        string FtpDownloadingTheMostRecentFileRemoteDir();

        void PasswordValidation(string currentPassword);
        void ChangeAuthenticationPassword(string currentPassword, string newPassword);
    }
}