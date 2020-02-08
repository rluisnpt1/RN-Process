using System;
using System.Collections.Generic;
using RN_Process.Shared.Enums;

namespace RN_Process.Api.DataAccess.Entities
{
    public interface IOrganization
    {
        string OrgCode { get; }
        string Description { get; }
        ICollection<ITerm> Terms { get; }

        void AddNewTerm(int termNumber, int typeDebt, TermsType termsType, FileAccessType communicationType,
            string internalHost,
            string linkToAccess, string linkToAccessType, string typeOfResponse,
            bool requiredLogin, string authenticationLogin, string authenticationPassword,
            string hostKeyFingerPrint, string authenticationCodeApp, string pathToOriginFile,
            string pathToDestinationFile, string pathToFileBackupAtClient,
            string pathToFileBackupAtHostServer, string fileDeLimiter,
            bool hashearder, string fileProtectedPassword,
            IList<string> fileHeaderColumns, IList<string> availableFieldsColumns);

        void AddTerm(string id, int termNumber, int typeDebt, TermsType termsType,
            FileAccessType communicationType, string internalHost,
            string linkToAccess, string linkToAccessType, string typeOfResponse,
            bool requiredLogin, string authenticationLogin, string authenticationPassword,
            string hostKeyFingerPrint, string authenticationCodeApp, string pathToOriginFile,
            string pathToDestinationFile, string pathToFileBackupAtClient,
            string pathToFileBackupAtHostServer, string fileDeLimiter,
            bool hashearder, string fileProtectedPassword,
            IList<string> fileHeaderColumns, IList<string> availableFieldsColumns);

        void RemoveTerms(string id) //, bool softDelete)
            ; bool Deleted { get; set; }

        bool Active { get; set; }

        /// <summary>
        /// </summary>
        String Id { get; set; }
    }
}