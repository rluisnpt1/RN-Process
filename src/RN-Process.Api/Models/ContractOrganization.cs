using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;
using RN_Process.Shared.Enums;

namespace RN_Process.Api.Models
{
    public class ContractOrganization
    {
        public ContractOrganization()
        {
            CodOrg = string.Empty;
            Description = string.Empty;
            DueDetails = new List<DueDetail>();
            DueDetailConfigs = new List<DueDetailConfiguration>();
        }

        public string Id { get; set; }

        /// <summary>
        ///     Cod organization
        /// </summary>
        public string CodOrg { get; set; }

        /// <summary>
        ///     Name that describe the organization
        /// </summary>
        public string Description { get; set; }

        public int ContractNumber { get; set; }

        public string DueId { get; set; }

        [JsonIgnore]
        public bool IsDeleted { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? ChangedDate { get; set; }

        public List<DueDetail> DueDetails { get; set; }
        public List<DueDetailConfiguration> DueDetailConfigs { get; set; }

        public void AddDueDetail(int debtCode, TermsType termsType)
        {
            DueDetails.Add(new DueDetail()
            {
                DebtCode = debtCode,
                TermsType = termsType
            });
        }

        public void AddDueDetailConfigs(string id,FileAccessType communicationType, string linkToAccess, string linkToAccessType,
            string typeOfResponse, bool requiredLogin, string authenticationLogin, string authenticationPassword,
            string hostkeyFingerPrint, string authenticationCodeApp, string pathToOriginFile,
            string pathToDestinationFile, string pathToFileBackupAtClient, string fileDelimiter,
            IList<string> fileHeaderColumns, IList<string> availableFieldsColumns)
        {
            DueDetailConfigs.Add(new DueDetailConfiguration()
            {
                Id = id,
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
                AvailableFieldsColumns = availableFieldsColumns
            });
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}