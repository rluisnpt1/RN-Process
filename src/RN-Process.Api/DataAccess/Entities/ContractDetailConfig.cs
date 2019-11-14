using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RN_Process.Api.Models;
using RN_Process.DataAccess;
using RN_Process.DataAccess.FTP;
using RN_Process.Shared.Commun;
using RN_Process.Shared.Enums;

namespace RN_Process.Api.DataAccess.Entities
{
    public sealed class ContractDetailConfig : AuditableEntity<string>
    {
        [BsonIgnore] private ICollection<FileImport> _fileImport;


        #region Ctors

        /// <summary>
        ///     Runtime execution
        /// </summary>
        private ContractDetailConfig()
        {
        }


        public ContractDetailConfig(Contract contract, FileAccessType communicationType,
            string internalHost,
            string baseWorkDirectoryHost,
            string linkToAccess,
            string linkToAccessType,
            string typeOfResponse,
            bool requiredLogin,
            string authenticationLogin,
            string authenticationPassword,
            string authenticationCodeApp,
            string pathToOriginFile,
            string pathToDestinationFile,
            string pathToFileBackupAtClient,
            string pathToFileBackupAtHostServer,
            string fileDeLimiter,
            IList<string> fileHeaderColumns,
            IList<string> availableFieldsColumns)
        {
            Id = ObjectId.GenerateNewId().ToString();
            SetContract(contract);
            SetCommunicationType(communicationType);
            InternalHost = internalHost;
            SetBaseWorkDirectoryHost(baseWorkDirectoryHost);
            LinkToAccess = linkToAccess;
            LinkToAccessType = linkToAccessType;
            TypeOfResponse = typeOfResponse;
            RequiredLogin = requiredLogin;
            SetLogin(authenticationLogin);
            SetPassword(authenticationPassword);
            SetAuthenticationApp(authenticationCodeApp);
            SetDirectoryFileOrigem(pathToOriginFile);
            PathToDestinationFile = pathToDestinationFile;
            SetBackupClientDirectory(pathToFileBackupAtClient);
            SetBackupHostServer(pathToFileBackupAtHostServer);
            SetDelimiter(fileDeLimiter);
            SetFileHeaderColumns(fileHeaderColumns);
            SetAvailableFieldsColumns(availableFieldsColumns);
            SetDirectoriesSize();

            Active = true;
            Deleted = false;
            RowVersion = new byte[0];
        }


        #endregion

        [BsonRepresentation(BsonType.ObjectId)]
        public string ContractId { get; private set; }
        public Contract Contract { get; set; }
        public string OrgCode { get; private set; }

        public FileAccessType CommunicationType { get; private set; }
        public string InternalHost { get; private set; }
        public string BaseWorkDirectoryHost { get; private set; }
        public string LinkToAccess { get; private set; }
        public string LinkToAccessType { get; private set; }
        public string TypeOfResponse { get; private set; }

        public bool RequiredLogin { get; private set; }
        public string AuthenticationLogin { get; private set; }
        public byte[] AuthenticationPassword { get; private set; }
        public string AuthenticationCodeApp { get; private set; }

        public string PathToOriginFile { get; private set; }
        public string PathToDestinationFile { get; private set; }
        public string PathToFileBackupAtClient { get; private set; }
        public string PathToFileBackupAtHostServer { get; private set; }
        public string FileDelimiter { get; private set; }

        public int DirectoryHostServerSize { get; private set; }

        public IList<string> FileHeaderColumns { get; private set; }
        public IList<string> AvailableFieldsColumns { get; private set; }

        public ICollection<FileImport> FileImports
        {
            get { return _fileImport ??= new List<FileImport>(); }
            protected set => _fileImport = value;
        }

        #region Sets

        private void SetDirectoriesSize()
        {
            if (!string.IsNullOrEmpty(BaseWorkDirectoryHost))
            {
                var directoryInfo = new DirectoryInfo(BaseWorkDirectoryHost);
                DirectoryHostServerSize = directoryInfo.GetDirectories().Length;
            }
        }


        private void SetDirectoryFileOrigem(string pathToOriginFile)
        {
            PathToOriginFile = pathToOriginFile;
        }

        private void SetBackupHostServer(string pathToFileBackupAtHostServer)
        {
            var clientDir = "\\backup\\" + OrgCode.ToUpper() + "\\";

            if (string.IsNullOrEmpty(pathToFileBackupAtHostServer))
                PathToFileBackupAtClient = IntrumFile.CreateDirectory(PathToOriginFile + clientDir);
            else
                PathToFileBackupAtHostServer = IntrumFile.CreateDirectory(pathToFileBackupAtHostServer + clientDir);
        }

        private void SetBaseWorkDirectoryHost(string baseWorkDirectoryHost)
        {
            var clientDir = "\\" + OrgCode.ToUpper() + "\\";

            BaseWorkDirectoryHost = string.IsNullOrEmpty(baseWorkDirectoryHost)
                ? IntrumFile.CreateDirectory(RnProcessConstant.BaseWorkFolder + clientDir)
                : IntrumFile.CreateDirectory(baseWorkDirectoryHost + clientDir);
        }

        private void SetBackupClientDirectory(string pathToFileBackupAtClient)
        {
            PathToFileBackupAtClient = pathToFileBackupAtClient;
        }

        private void SetAvailableFieldsColumns(IList<string> availableFieldsColumns)
        {
            Guard.Against.Null(availableFieldsColumns, nameof(availableFieldsColumns));
            Guard.Against.Zero(availableFieldsColumns.Count, nameof(availableFieldsColumns));
            if (availableFieldsColumns.Contains(" "))
                throw new EncoderFallbackException("list can not have empty strings");

            AvailableFieldsColumns = availableFieldsColumns;
        }

        private void SetFileHeaderColumns(IList<string> fileHeaderColumns)
        {
            Guard.Against.Null(fileHeaderColumns, nameof(fileHeaderColumns));
            Guard.Against.Zero(fileHeaderColumns.Count, nameof(fileHeaderColumns));
            if (fileHeaderColumns.Contains(" "))
                throw new EncoderFallbackException("list can not have empty strings");

            FileHeaderColumns = fileHeaderColumns;
        }

        private void SetCommunicationType(FileAccessType communicationType)
        {
            Guard.Against.Null(communicationType, nameof(communicationType));
            Guard.Against.NullOrEmpty(communicationType.ToString(), nameof(communicationType));
            CommunicationType = communicationType;
        }


        private void SetDelimiter(string fileDeLimiter)
        {
            var result = string.IsNullOrEmpty(fileDeLimiter) ? "," : fileDeLimiter;
            FileDelimiter = result;
        }

        private void SetContract(Contract contract)
        {
            Guard.Against.Null(contract, nameof(contract));
            ContractId = contract.Id;
            OrgCode = contract.OrgCode;
            Contract = contract;
        }

        private void SetAuthenticationApp(string authenticationCodeApp)
        {
            AuthenticationCodeApp = authenticationCodeApp;
        }

        private void SetLogin(string authenticationLogin)
        {
            if (AuthenticationLogin == null && RequiredLogin == false && !string.IsNullOrEmpty(authenticationLogin))
                RequiredLogin = true;

            if (RequiredLogin)
            {
                Guard.Against.NullOrEmpty(authenticationLogin, nameof(authenticationLogin));
                Guard.Against.NullOrWhiteSpace(authenticationLogin, nameof(authenticationLogin));
            }

            AuthenticationLogin = authenticationLogin;
        }

        private void SetPassword(string password)
        {
            if (AuthenticationPassword == null && RequiredLogin == false && !string.IsNullOrEmpty(password))
                RequiredLogin = true;

            if (RequiredLogin)
            {
                Guard.Against.NullOrEmpty(password, nameof(password));
                Guard.Against.NullOrWhiteSpace(password, nameof(password));
            }

            AuthenticationPassword =
                Encoding.ASCII.GetBytes(password); //CriptografiaHelper.PasswordCryptography(password);
        }


        public void PasswordValidation(string currentPassword)
        {
            Guard.Against.NullOrEmpty(currentPassword, nameof(currentPassword));
            var encryptedPassword = CriptografiaHelper.PasswordCryptography(currentPassword);

            if (!AuthenticationPassword.SequenceEqual(encryptedPassword))
                throw new Exception("invalid password!");
        }

        public void ChangeAuthenticationPassword(string currentPassword, string newPassword)
        {
            PasswordValidation(currentPassword);
            SetPassword(newPassword);
        }

        #endregion

        public void CreateCommunicationType()
        {
            Guard.Against.NullOrWhiteSpace(BaseWorkDirectoryHost, nameof(BaseWorkDirectoryHost));

            if (!Directory.Exists(BaseWorkDirectoryHost)) throw new Exception($"ERROR: {BaseWorkDirectoryHost} does not exist");
            else
                switch (CommunicationType)
                {
                    case FileAccessType.FTP:
                        var ftpLogin = new FtpClient(AuthenticationLogin, Encoding.ASCII.GetString(AuthenticationPassword), LinkToAccess);
                        CreateCommunicationWithFTP(ftpLogin);
                        break;
                    case FileAccessType.Email:
                        break;
                    case FileAccessType.WebServer:
                        break;
                    case FileAccessType.API:
                        break;
                    case FileAccessType.WebSite:
                        break;
                    case FileAccessType.DataBase:
                        break;
                    case FileAccessType.RemoteDesktop:
                        break;
                    case FileAccessType.ActiveDirectory:
                        break;
                    case FileAccessType.LocalMachine:
                        CreateCommunicationWithLocalMachine();
                        break;
                }
        }

        private void CreateCommunicationWithLocalMachine()
        {
            Guard.Against.NullOrWhiteSpace(PathToOriginFile,nameof(PathToOriginFile));
            Guard.Against.NullOrWhiteSpace(PathToDestinationFile,nameof(PathToDestinationFile));
            var fileProcessor = new FileProcessor(PathToOriginFile);
            fileProcessor.Process();
        }


        private void CreateCommunicationWithFTP(FtpClient ftpLogin)
        {
            ftpLogin.CreateCredential("");
            var stringData = ftpLogin.DirectoryListSimple(PathToOriginFile);

        }


        //public Guid GenerateNewTokenToChangePassword()
        //{
        //    TokenAlteracaoDeSenha = Guid.NewGuid();
        //    return TokenAlteracaoDeSenha;
        //}

        //public void ChangePassword(Guid token, string novaSenha)
        //{
        //    if (!TokenAlteracaoDeSenha.Equals(token))
        //        throw new Exception("token para alteração de senha inválido!");
        //    SetPassword(novaSenha);
        //    GenerateNewTokenToChangePassword();
        //}
    }
}