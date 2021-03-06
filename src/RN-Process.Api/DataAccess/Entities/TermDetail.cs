﻿using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RN_Process.DataAccess;
using RN_Process.Shared.Commun;
using RN_Process.Shared.Enums;

namespace RN_Process.Api.DataAccess.Entities
{
    public class TermDetail : AuditableEntity<string>, ITermDetail
    {
        [BsonIgnore] private ICollection<ITermDetailConfig> _termConfig;

        public TermDetail(int debtCode, TermsType name, ITerm term)
        {
            Id = ObjectId.GenerateNewId().ToString();
            SetDebtCode(debtCode);
            SerTermTypeName(name);
            SetTerm(term);
            Active = true;
            Deleted = false;
            RowVersion = new byte[0];
        }

        public int DebtCode { get; private set; }
        public TermsType TermsType { get; private set; }
        public string OrgCode { get; private set; }

        public string TermId { get; private set; }

        [BsonIgnore] public ITerm Term { get; set; }

        public virtual ICollection<ITermDetailConfig> TermDetailConfigs
        {
            get { return _termConfig ??= new List<ITermDetailConfig>(); }
            protected set => _termConfig = value;
        }

        private void SerTermTypeName(TermsType name)
        {
            Guard.Against.Null(name, nameof(TermsType));
            TermsType = name;
        }

        private void SetDebtCode(int debtCode)
        {
            Guard.Against.Zero(debtCode, nameof(debtCode));
            DebtCode = debtCode;
        }

        private void SetTerm(ITerm term)
        {
            Guard.Against.Null(term, nameof(term));
            Term = term;
            TermId = term.Id;
            OrgCode = term.OrgCode;
        }

        public void AddDetailConfig(string id, FileAccessType communicationType, string internalHost,
            string linkToAccess, string linkToAccessType, string typeOfResponse,
            bool requiredLogin, string authenticationLogin, string authenticationPassword,
            string hostKeyFingerPrint, string authenticationCodeApp, string pathToOriginFile,
            string pathToDestinationFile, string pathToFileBackupAtClient,
            string pathToFileBackupAtHostServer, string fileDeLimiter,
            bool hashearder, string fileProtectedPassword,
            IList<string> fileHeaderColumns, IList<string> availableFieldsColumns,
            bool active = true)
        {
            if (!string.IsNullOrWhiteSpace(id))
                UpdateTermConfiguration(id, communicationType, internalHost, linkToAccess, linkToAccessType,
                    typeOfResponse, requiredLogin, authenticationLogin, authenticationPassword,
                    hostKeyFingerPrint, authenticationCodeApp, pathToOriginFile,
                    pathToDestinationFile, pathToFileBackupAtClient, pathToFileBackupAtHostServer,
                    fileDeLimiter,hashearder ,fileProtectedPassword,fileHeaderColumns, availableFieldsColumns, active);
            else
                AddNewTermDetailConfig(communicationType, internalHost, linkToAccess, linkToAccessType, typeOfResponse,
                    requiredLogin, authenticationLogin, authenticationPassword, hostKeyFingerPrint,
                    authenticationCodeApp, pathToOriginFile, pathToDestinationFile,
                    pathToFileBackupAtClient, pathToFileBackupAtHostServer, fileDeLimiter,hashearder,
                    fileProtectedPassword,fileHeaderColumns, availableFieldsColumns);
        }


        public void AddNewTermDetailConfig(FileAccessType communicationType, string internalHost, string linkToAccess,
            string linkToAccessType, string typeOfResponse, bool requiredLogin,
            string authenticationLogin, string authenticationPassword,
            string hostKeyFingerPrint, string authenticationCodeApp,
            string pathToOriginFile, string pathToDestinationFile,
            string pathToFileBackupAtClient, string pathToFileBackupAtHostServer,
            string fileDeLimiter,
            bool hashearder, string fileProtectedPassword,
            IList<string> fileHeaderColumns,
            IList<string> availableFieldsColumns)
        {
            var fact = CreateConfiguration(communicationType, internalHost, linkToAccess, linkToAccessType,
                typeOfResponse, requiredLogin, authenticationLogin, authenticationPassword,
                hostKeyFingerPrint, authenticationCodeApp, pathToOriginFile,
                pathToDestinationFile, pathToFileBackupAtClient, pathToFileBackupAtHostServer,
                fileDeLimiter, hashearder, fileProtectedPassword, fileHeaderColumns, availableFieldsColumns);

            TermDetailConfigs.Add(fact);
        }


        public void UpdateTermConfiguration(string id, FileAccessType communicationType, string internalHost,
            string linkToAccess, string linkToAccessType, string typeOfResponse,
            bool requiredLogin, string authenticationLogin,
            string authenticationPassword, string hostKeyFingerPrint,
            string authenticationCodeApp, string pathToOriginFile,
            string pathToDestinationFile, string pathToFileBackupAtClient,
            string pathToFileBackupAtHostServer, string fileDeLimiter,
            bool hashearder, string fileProtectedPassword,
            IList<string> fileHeaderColumns, IList<string> availableFieldsColumns,
            bool active = true)
        {
            ITermDetailConfig config = null;
            var foundIt = false;

            if (!string.IsNullOrEmpty(id))
                config = TermDetailConfigs.FirstOrDefault(temp => temp.Id.Equals(id)
                                                                  && temp.TermDetailId == Id
                                                                  && temp.TermDetail.OrgCode == OrgCode);

            if (config == null)
            {
                config = CreateConfiguration(communicationType, internalHost, linkToAccess, linkToAccessType,
                    typeOfResponse, requiredLogin, authenticationLogin, authenticationPassword,
                    hostKeyFingerPrint, authenticationCodeApp, pathToOriginFile,
                    pathToDestinationFile, pathToFileBackupAtClient,
                    pathToFileBackupAtHostServer, fileDeLimiter, hashearder, fileProtectedPassword, fileHeaderColumns,
                    availableFieldsColumns);
            }
            else
            {
                foundIt = true;
                //config.UpdatedDate = DateTime.UtcNow;
                //config.ModifiedBy = "System-- need change for user";
                config.Active = active;
                config.Deleted = !active;
            }

            if (foundIt == false) TermDetailConfigs.Add(config);
        }

        private TermDetailConfig CreateConfiguration(FileAccessType communicationType, string internalHost,
            string linkToAccess, string linkToAccessType, string typeOfResponse,
            bool requiredLogin, string authenticationLogin,
            string authenticationPassword, string hostKeyFingerPrint,
            string authenticationCodeApp, string pathToOriginFile,
            string pathToDestinationFile, string pathToFileBackupAtClient,
            string pathToFileBackupAtHostServer, string fileDeLimiter,
            bool hashearder, string fileProtectedPassword,
            IList<string> fileHeaderColumns,
            IList<string> availableFieldsColumns)
        {
            var fact = new TermDetailConfig(ObjectId.GenerateNewId().ToString(), this, communicationType, internalHost,
                linkToAccess, linkToAccessType, typeOfResponse, requiredLogin,
                authenticationLogin, authenticationPassword, hostKeyFingerPrint,
                authenticationCodeApp, pathToOriginFile, pathToDestinationFile,
                pathToFileBackupAtClient, pathToFileBackupAtHostServer, fileDeLimiter, hashearder, fileProtectedPassword, string.Empty, fileHeaderColumns, availableFieldsColumns);
            return fact;
        }
    }
}