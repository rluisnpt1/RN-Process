using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using RN_Process.Shared.Enums;

namespace RN_Process.Api.Models
{
    public class ContractOrganization
    {
        public ContractOrganization()
        {
            CodOrg = string.Empty;
            Description = string.Empty;
            ContractNumber = 0;
            Dues = new List<Due>();
        }
        public string Id { get; set; }
        /// <summary>
        /// Cod organization
        /// </summary>
        public string CodOrg { get;  set; }

        /// <summary>
        /// Name that describe the organization
        /// </summary>
        public string Description { get;  set; }

        /// <summary>
        /// Number uniq that identify the contract document paper 
        /// </summary>
        public int ContractNumber { get; set; }


        public List<Due> Dues { get; private set; }

        public void AddDues(int debtCode, TermsType termsType, FileAccessType communicationType, string linkToAccess,
            string linkToAccessType, string typeOfResponse, bool requiredLogin, string authenticationLogin,
            byte[] authenticationPassword, string hostkeyFingerPrint, string authenticationCodeApp,
            string pathToOriginFile, string pathToDestinationFile, string pathToFileBackupAtClient,
            string fileDelimiter, IList<string> fileHeaderColumns, IList<string> availableFieldsColumns, bool isDeleted)
        {
        
            Dues.Add(new Due()
            {
                DebtCode = debtCode,
                TermsType = termsType,
                CommunicationType = communicationType,
                LinkToAccess = linkToAccess,
                LinkToAccessType = linkToAccessType,
                TypeOfResponse = typeOfResponse,
                RequiredLogin = requiredLogin,
                AuthenticationLogin = authenticationLogin,
                AuthenticationPassword = authenticationPassword,
                HostkeyFingerPrint = hostkeyFingerPrint,
                AuthenticationCodeApp = authenticationCodeApp,
                PathToOriginFile = pathToOriginFile,
                PathToDestinationFile = pathToDestinationFile,
                PathToFileBackupAtClient = pathToFileBackupAtClient,
                FileDelimiter = fileDelimiter,
                FileHeaderColumns = fileHeaderColumns,
                AvailableFieldsColumns = availableFieldsColumns,
                IsDeleted = isDeleted
            });
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

    }
}
