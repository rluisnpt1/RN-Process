
using System;
using System.Collections.Generic;
using System.Text;
using RN_Process.DataAccess;
using RN_Process.Shared.Commun;

namespace RN_Process.Api.Models
{
    public class ContractMappingBase : EntityMdb<string>
    {

        public virtual List<FileImport> FileImports { get; set; }


        public string CodReference { get; set; }
        public string InternalHost { get; set; }
        public string LinkToAccess { get; set; }
        public string LinkToAccesTipo { get; set; }
        public string TypeOfResponse { get; set; }
        public bool RequiredLogin { get; set; }
        public string AuthenticationLogin { get; set; }
        public string AuthenticationPassword { get; set; }
        public string AuthenticationCodeApp { get; set; }
        public string PathToOriginFile { get; set; }
        public string PathToDestinationFile { get; set; }
        public string PathToFileBackupAtClient { get; set; }
        public string PathToFileBackupAtHostServer { get; set; }
        public char[] FileDeLimiter { get; set; }
        public string ContractId { get; private set; }
        public virtual Contract Contract { get; set; }


        public ContractMappingBase(string codReference, string internalHost, string linkToAccess, string linkToAccesTipo, string typeOfResponse, bool requiredLogin, string authenticationLogin, string authenticationPassword, string authenticationCodeApp, string pathToOriginFile, string pathToDestinationFile, string pathToFileBackupAtClient, string pathToFileBackupAtHostServer, char[] fileDeLimiter, Contract contract)
        {
            SetContract(contract);
            CodReference = codReference;
            InternalHost = internalHost;
            LinkToAccess = linkToAccess;
            LinkToAccesTipo = linkToAccesTipo;
            TypeOfResponse = typeOfResponse;
            RequiredLogin = requiredLogin;
            AuthenticationLogin = authenticationLogin;
            AuthenticationPassword = authenticationPassword;
            AuthenticationCodeApp = authenticationCodeApp;
            PathToOriginFile = pathToOriginFile;
            PathToDestinationFile = pathToDestinationFile;
            PathToFileBackupAtClient = pathToFileBackupAtClient;
            PathToFileBackupAtHostServer = pathToFileBackupAtHostServer;
            FileDeLimiter = fileDeLimiter;
            FileImports = new List<FileImport>();
        }
        private void SetContract(Contract contract)
        {
            Guard.Against.Null(contract, nameof(contract));
            ContractId = contract.Id;
            Contract = contract;
        }
    }
}
