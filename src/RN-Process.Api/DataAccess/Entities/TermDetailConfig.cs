using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RN_Process.DataAccess;
using RN_Process.Shared.Commun;
using RN_Process.Shared.Enums;
using WinSCP;

namespace RN_Process.Api.DataAccess.Entities
{
    public class TermDetailConfig : AuditableEntity<string>
    {
        private static readonly string
            BaseWorkDir = "C:\\TEMP\\WorkDir"; //Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        [BsonIgnore] private ICollection<OrganizationFile> _fileImport;

        [BsonRepresentation(BsonType.ObjectId)]
        //public string TermId { get; private set; }
        //public Term Term { get; set; }   

        public string TermDetailId { get; private set; }

        public TermDetail TermDetail { get; set; }


        public string OrgCode { get; private set; }

        public FileAccessType CommunicationType { get; private set; }
        public string InternalHost { get; }
        public string BaseWorkDirectoryHost { get; private set; }
        public string LinkToAccess { get; }
        public string LinkToAccessType { get; }
        public string TypeOfResponse { get; }

        public bool RequiredLogin { get; private set; }
        public string AuthenticationLogin { get; private set; }
        public byte[] AuthenticationPassword { get; private set; }
        public byte[] HostKeyFingerPrint { get; private set; }
        public string AuthenticationCodeApp { get; private set; }

        public string PathToOriginFile { get; private set; }
        public string PathToDestinationFile { get; }
        public string PathToFileBackupAtClient { get; private set; }
        public string PathToFileBackupAtHostServer { get; private set; }
        public string FileDelimiter { get; private set; }

        public int DirectoryHostServerSize { get; private set; }

        public IList<string> FileHeaderColumns { get; private set; }
        public IList<string> AvailableFieldsColumns { get; private set; }

        public ICollection<OrganizationFile> OrganizationFiles
        {
            get { return _fileImport ??= new List<OrganizationFile>(); }
            protected set => _fileImport = value;
        }

        public void AddOrganizationFile(string id, string orgCode, string fileDescription, int fileSize, string fileFormat,
            string fileLocationOrigin, string locationToCopy, StatusType status, bool fileMigrated,
            DateTime? fileMigratedOn, List<BsonDocument> allDataInFile, bool active)
        {
            if (!string.IsNullOrEmpty(id))
                UpdateExistingTermById(id, orgCode,status, fileMigrated, fileMigratedOn, active);
            else
                AddNwwOrganizationFile("", fileDescription, fileSize, fileFormat, fileLocationOrigin
                    , locationToCopy, status, allDataInFile);
        }

        private void UpdateExistingTermById(string id, string orgCode, StatusType status, 
            bool fileMigrated, DateTime? fileMigratedOn, bool active)
        {
            OrganizationFile orgFile =null;
            var foundIt = false;
            if (!string.IsNullOrEmpty(id)) orgFile = 
                OrganizationFiles.FirstOrDefault(temp => temp.Id == id
                                                         && temp.OrgCode.ToUpper()
                                                             .Equals(orgCode.ToUpper()));
            if (orgFile != null)
            {
                foundIt = true;
                orgFile.Status = status;
                orgFile.FileMigrated = fileMigrated;
                orgFile.FileMigratedOn = fileMigratedOn;
                orgFile.Active = active;
                orgFile.Deleted = !active;

                OrganizationFiles.Add(orgFile);
            }
        }

        private void AddNwwOrganizationFile(string id, string fileDescription, int fileSize,
            string fileFormat, string fileLocationOrigin, string locationToCopy, StatusType status, 
            List<BsonDocument> allDataInFile)
        {
            var newdoc = new OrganizationFile(id, fileDescription, fileSize, fileFormat, fileLocationOrigin
                , locationToCopy, status, false, null, this, allDataInFile);
           
            OrganizationFiles.Add(newdoc);
        }

        /// <summary>
        ///     Downloading the most recent file
        /// </summary>
        public string FtpDownloadingTheMostRecentFileRemoteDir()
        {
            Guard.Against.NullOrWhiteSpace(BaseWorkDirectoryHost, nameof(BaseWorkDirectoryHost));
            if (!Directory.Exists(BaseWorkDirectoryHost))
                throw new Exception($"ERROR: {BaseWorkDirectoryHost} does not exist");

            if (CommunicationType != FileAccessType.FTP)
                throw new Exception($"ERROR: Method available only for {FileAccessType.FTP}");

            var sessionOptionsLogin = FtpWork(out var options);

            var directoryInfo = sessionOptionsLogin.GetRemoteDirectoryInfo(options, PathToOriginFile);
            var latest = sessionOptionsLogin.GetLastFileRemoteFileInfo(directoryInfo);

            var destination = BaseWorkDirectoryHost + "\\" + latest.Name;
            sessionOptionsLogin.DownloadFileRemoteDir(options, latest.FullName, destination);

            return destination;
        }


        protected virtual FtpWork FtpWork(out SessionOptions options)
        {
            var sessionOptionsLogin = new FtpWork();
            options = sessionOptionsLogin.SessionOptions(LinkToAccess,
                AuthenticationLogin,
                Encoding.ASCII.GetString(AuthenticationPassword),
                Encoding.ASCII.GetString(HostKeyFingerPrint));
            return sessionOptionsLogin;
        }


        #region Ctors

        /// <summary>
        ///     Runtime execution
        /// </summary>
        private TermDetailConfig()
        {
        }


        public TermDetailConfig(string id, TermDetail termDetail, FileAccessType communicationType,
            string internalHost,
            string linkToAccess,
            string linkToAccessType,
            string typeOfResponse,
            bool requiredLogin,
            string authenticationLogin,
            string authenticationPassword,
            string hostKeyFingerPrint,
            string authenticationCodeApp,
            string pathToOriginFile,
            string pathToDestinationFile,
            string pathToFileBackupAtClient,
            string pathToFileBackupAtHostServer,
            string fileDeLimiter,
            IList<string> fileHeaderColumns,
            IList<string> availableFieldsColumns)
        {
            Id = string.IsNullOrWhiteSpace(id) ? ObjectId.GenerateNewId().ToString() : id;
            SetTermDetail(termDetail);
            SetCommunicationType(communicationType);
            InternalHost = internalHost;
            SetBaseWorkDirectoryHost();
            LinkToAccess = linkToAccess;
            LinkToAccessType = linkToAccessType;
            TypeOfResponse = typeOfResponse;
            RequiredLogin = requiredLogin;
            SetLogin(authenticationLogin);
            SetPassword(authenticationPassword);
            SetHostKeyFingerPrint(hostKeyFingerPrint);
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

        #region Sets

        private void SetDirectoriesSize()
        {
            if (!string.IsNullOrEmpty(BaseWorkDirectoryHost))
            {
                var directoryInfo = new DirectoryInfo(BaseWorkDirectoryHost);

                if (directoryInfo.Parent != null)
                    DirectoryHostServerSize = directoryInfo.Parent.GetDirectories().Length;
            }
        }


        private void SetDirectoryFileOrigem(string pathToOriginFile)
        {
            PathToOriginFile = pathToOriginFile;
        }

        private void SetBackupHostServer(string pathToFileBackupAtHostServer)
        {
            var clientDir = $"\\backup\\{OrgCode.ToUpper()}";

            if (string.IsNullOrEmpty(pathToFileBackupAtHostServer))
            {
                var te2 = PathToOriginFile.Replace("/", "");
                PathToFileBackupAtClient = IntrumFile.CreateDirectory(te2 + clientDir);
            }
            else
            {
                PathToFileBackupAtHostServer = IntrumFile.CreateDirectory(pathToFileBackupAtHostServer + clientDir);
            }
        }

        private void SetBaseWorkDirectoryHost()
        {
            var clientDir = $"\\ToProcess\\{OrgCode.ToUpper()}";

            BaseWorkDirectoryHost = IntrumFile.CreateDirectory(BaseWorkDir + clientDir);
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

        private void SetTermDetail(TermDetail term)
        {
            Guard.Against.Null(term, nameof(TermDetail));
            TermDetailId = term.Id;
            OrgCode = term.OrgCode;
            TermDetail = term;
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

        private void SetHostKeyFingerPrint(string hostKeyFingerPrint)
        {
            HostKeyFingerPrint = Encoding.ASCII.GetBytes(hostKeyFingerPrint);
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