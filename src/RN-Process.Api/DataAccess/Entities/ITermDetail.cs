using System;
using System.Collections.Generic;
using RN_Process.Shared.Enums;

namespace RN_Process.Api.DataAccess.Entities
{
    public interface ITermDetail
    {
        int DebtCode { get; }
        TermsType TermsType { get; }
        string OrgCode { get; }
        string TermId { get; }
        ITerm Term { get; set; }

        ICollection<ITermDetailConfig> TermDetailConfigs { get; }

        /// <summary>
        /// </summary>
        bool Deleted { get; set; }

        bool Active { get; set; }

        /// <summary>
        /// </summary>
        String Id { get; set; }

        void AddDetailConfig(string id, FileAccessType communicationType, string internalHost,
            string linkToAccess, string linkToAccessType, string typeOfResponse,
            bool requiredLogin, string authenticationLogin, string authenticationPassword,
            string hostKeyFingerPrint, string authenticationCodeApp, string pathToOriginFile,
            string pathToDestinationFile, string pathToFileBackupAtClient,
            string pathToFileBackupAtHostServer, string fileDeLimiter,
            bool hashearder, string fileProtectedPassword,
            IList<string> fileHeaderColumns, IList<string> availableFieldsColumns,
            bool active = true);

        void AddNewTermDetailConfig(FileAccessType communicationType, string internalHost, string linkToAccess,
            string linkToAccessType, string typeOfResponse, bool requiredLogin,
            string authenticationLogin, string authenticationPassword,
            string hostKeyFingerPrint, string authenticationCodeApp,
            string pathToOriginFile, string pathToDestinationFile,
            string pathToFileBackupAtClient, string pathToFileBackupAtHostServer,
            string fileDeLimiter,
            bool hashearder, string fileProtectedPassword,
            IList<string> fileHeaderColumns,
            IList<string> availableFieldsColumns);

        void UpdateTermConfiguration(string id, FileAccessType communicationType, string internalHost,
            string linkToAccess, string linkToAccessType, string typeOfResponse,
            bool requiredLogin, string authenticationLogin,
            string authenticationPassword, string hostKeyFingerPrint,
            string authenticationCodeApp, string pathToOriginFile,
            string pathToDestinationFile, string pathToFileBackupAtClient,
            string pathToFileBackupAtHostServer, string fileDeLimiter,
            bool hashearder, string fileProtectedPassword,
            IList<string> fileHeaderColumns, IList<string> availableFieldsColumns,
            bool active = true);
    }
}