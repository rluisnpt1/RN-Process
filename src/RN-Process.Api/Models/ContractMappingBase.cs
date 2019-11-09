
using System;
using System.Collections.Generic;
using System.Text;
using RN_Process.DataAccess;

namespace RN_Process.Api.Models
{
    public class ContractMappingBase : EntityMdb<string>
    {
        public ContractMappingBase(string codReference, string internalHost, string linkToAccess, string linkToAccesTipo, string typeOfResponse, bool requiredLogin, string authenticationLogin, string authenticationPassword, string authenticationCodeApp, string pathToOriginFile, string pathToDestinationFile, string pathToFileBackupAtClient, string pathToFileBackupAtHostServer, char[] fileDeLimiter, Contract contract, Customer customer)
        {
            
            SetUniqCodeCustomer();
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
            Contract = contract;
            Customer = customer;
        }

        private void SetUniqCodeCustomer()
        {
            UniqCodeCustomer = Customer.UniqCode;
        }

        public string UniqCodeCustomer { get; set; }

        public string CodReference { get; set; }

        /// <summary>
        /// store the link where the main server is
        /// </summary>
        public string InternalHost { get; set; }
        /// <summary>
        /// LINK //FTP IP, WEB SERVER HTTP, WEB SITE HTTP
        /// </summary>
        public string LinkToAccess { get; set; }
        /// <summary>
        /// TIPO dE ACESSO // FTP ETL 
        /// </summary>
        public string LinkToAccesTipo { get; set; }
        /// <summary>
        /// type response made in contract - FTP - WEB SERVER ETC
        /// </summary>
        public string TypeOfResponse { get; set; }

        public bool RequiredLogin { get; set; }
        public string AuthenticationLogin { get; set; }
        public string AuthenticationPassword { get; set; }
        public string AuthenticationCodeApp { get; set; }

        /// <summary>
        /// Path to get the file
        /// </summary>
        public string PathToOriginFile { get; set; }
        /// <summary>
        /// Path to send the file in response
        /// </summary>
        public string PathToDestinationFile { get; set; }
        public string PathToFileBackupAtClient { get; set; }
        public string PathToFileBackupAtHostServer { get; set; }

        public char[] FileDeLimiter { get; set; }

   
        public virtual Contract Contract { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
