﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RN_Process.Shared.Enums;

namespace RN_Process.Api.Models
{
    public class DueDetail
    {
        public DueDetail()
        {
            DueDetailConfigs = new List<DueDetailConfiguration>();
        }

        public DueDetail(string id, int debtCode, string termsType)
        {
            Id = id;
            DebtCode = debtCode;
            TermsType = termsType;
            DueDetailConfigs = new List<DueDetailConfiguration>();
        }

        public string Id { get; set; }

        /// <summary>
        ///     Uniq code that identify the debt
        /// </summary>
        [Display(Name = "Debt Code (Intrum)")]
        [Required]
        public int DebtCode { get; set; }

        /// <summary>
        ///     Description of debt
        /// convert String in Enum - this works for bson
        /// </summary>
        [Display(Name = "Type Debt")]
        [Required]
       [JsonConverter(typeof(StringEnumConverter))]
        public string TermsType { get; set; }

        public List<DueDetailConfiguration> DueDetailConfigs { get; set; }

        public void AddDueDetailConfigs(string id, string communicationType, string linkToAccess,
            string linkToAccessType,
            string typeOfResponse, bool requiredLogin, string authenticationLogin, string authenticationPassword,
            string hostkeyFingerPrint, string authenticationCodeApp, string pathToOriginFile,
            string pathToDestinationFile, string pathToFileBackupAtClient, string fileDelimiter,
            bool hashearder, string fileProtectedPassword,
            IList<string> fileHeaderColumns, IList<string> availableFieldsColumns)
        {
            DueDetailConfigs.Add(new DueDetailConfiguration
            {
                Id = id,
                CommunicationType = communicationType.ToString(),
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
                HasHeader = hashearder,
                FileProtectedPassword = fileProtectedPassword,
                FileHeaderColumns = fileHeaderColumns,
                AvailableFieldsColumns = availableFieldsColumns
            });
        }
    }
}