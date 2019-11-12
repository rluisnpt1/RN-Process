using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RN_Process.DataAccess;
using RN_Process.Shared.Commun;

namespace RN_Process.Api.DataAccess.Entities
{
    public sealed class ContractDetailConfig : AuditableEntity<string>
    {
        [BsonIgnore] private ICollection<FileImport> _fileImport;


        /// <summary>
        ///     Runtime execution
        /// </summary>
        private ContractDetailConfig()
        {
        }


        public ContractDetailConfig(
            string communicationType,
            string internalHost,
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
            Contract contract)
        {
            Id = ObjectId.GenerateNewId().ToString();
            SetCommunicationType(communicationType);
            InternalHost = internalHost;
            LinkToAccess = linkToAccess;
            LinkToAccessType = linkToAccessType;
            TypeOfResponse = typeOfResponse;
            PathToOriginFile = pathToOriginFile;
            PathToDestinationFile = pathToDestinationFile;
            PathToFileBackupAtClient = pathToFileBackupAtClient;
            PathToFileBackupAtHostServer = pathToFileBackupAtHostServer;
            FileDelimiter = fileDeLimiter;
            FileHeaderColumns = fileHeaderColumns;

            RequiredLogin = requiredLogin;
            SetDelimiter(FileDelimiter);
            SetLogin(authenticationLogin);
            SetPassword(authenticationPassword);
            SetAuthenticationApp(authenticationCodeApp);
            SetContract(contract);

            Active = true;
            Deleted = false;
            RowVersion = new byte[0];
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string ContractId { get; private set; }

        public Contract Contract { get; set; }


        public string CommunicationType { get; set; }
        public string InternalHost { get; set; }
        public string LinkToAccess { get; set; }
        public string LinkToAccessType { get; set; }
        public string TypeOfResponse { get; set; }


        public bool RequiredLogin { get; set; }
        public string AuthenticationLogin { get; set; }
        public byte[] AuthenticationPassword { get; set; }
        public string AuthenticationCodeApp { get; set; }

        public string PathToOriginFile { get; set; }
        public string PathToDestinationFile { get; set; }
        public string PathToFileBackupAtClient { get; set; }
        public string PathToFileBackupAtHostServer { get; set; }
        public string FileDelimiter { get; set; }
        public IList<string> FileHeaderColumns { get; set; }

        public Guid TokenAlteracaoDeSenha { get; private set; }

        public ICollection<FileImport> FileImports
        {
            get { return _fileImport ??= new List<FileImport>(); }
            protected set => _fileImport = value;
        }

        private void SetCommunicationType(string communicationType)
        {
            Guard.Against.NullOrEmpty(communicationType, nameof(communicationType));
            Guard.Against.NullOrWhiteSpace(communicationType, nameof(communicationType));
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

            AuthenticationPassword = CriptografiaHelper.PasswordCryptography(password);
        }


        //public void PasswordValidation(string currentPassword)
        //{
        //    Guard.Against.NullOrEmpty(currentPassword, nameof(currentPassword));
        //    var encryptedPassword = CriptografiaHelper.PasswordCryptography(currentPassword);

        //    if (!AuthenticationPassword.SequenceEqual(encryptedPassword))
        //        throw new Exception("invalid password!");
        //}

        //public void ChangeAuthenticationPassword(string currentPassword, string newPassword)
        //{
        //    PasswordValidation(currentPassword);
        //    SetPassword(newPassword);
        //}
        //    public Guid GenerateNewTokenToChangePassword()
        //    {
        //        TokenAlteracaoDeSenha = Guid.NewGuid();
        //        return TokenAlteracaoDeSenha;
        //    }

        //    public void ChangePassword(Guid token, string novaSenha)
        //    {
        //        if (!TokenAlteracaoDeSenha.Equals(token))
        //            throw new Exception("token para alteração de senha inválido!");
        //        SetPassword(novaSenha);
        //        GenerateNewTokenToChangePassword();
        //    }
    }
}