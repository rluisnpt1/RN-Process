using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RN_Process.DataAccess;
using RN_Process.Shared.Commun;
using RN_Process.Shared.Enums;

namespace RN_Process.Api.DataAccess.Entities
{
  

    public class Term : AuditableEntity<string>, ITerm
    {
        [BsonIgnore] 
        private ICollection<ITermDetail> _termDetail;

        //Runtime execution
        protected Term()
        {
        }

        public Term(int termNumber, IOrganization organization)
        {
            Id = ObjectId.GenerateNewId().ToString();
            SetTermNumber(termNumber);
            SetCustomer(organization);
            Active = true;
            Deleted = false;
        }

        [BsonIgnore] 
        public virtual IOrganization Organization { get; set; }


        public int TermNumber { get; private set; }

        public virtual string OrgCode { get; private set; }
        public string OrganizationId { get; private set; }


        public virtual ICollection<ITermDetail> TermDetails
        {
            get { return _termDetail ??= new List<ITermDetail>(); }
            set => _termDetail = value;
        }

        private void SetCustomer(IOrganization organization)
        {
            Guard.Against.Null(organization, nameof(organization));
            OrganizationId = organization.Id;
            OrgCode = organization.OrgCode;
            Organization = organization;
        }

        private void SetTermNumber(int termNumber)
        {
            Guard.Against.Zero(termNumber, nameof(TermNumber));
            TermNumber = termNumber;
        }

        public void AddTermDetail(string id, int debtCode, TermsType termType,
            FileAccessType communicationType, string internalHost,
            string linkToAccess, string linkToAccessType, string typeOfResponse,
            bool requiredLogin, string authenticationLogin, string authenticationPassword,
            string hostKeyFingerPrint, string authenticationCodeApp, string pathToOriginFile,
            string pathToDestinationFile, string pathToFileBackupAtClient,
            string pathToFileBackupAtHostServer, string fileDeLimiter,
            bool hashearder, string fileProtectedPassword,
            IList<string> fileHeaderColumns, IList<string> availableFieldsColumns, bool active = true)
        {
            Guard.Against.Null(debtCode, nameof(debtCode));
            Guard.Against.Zero(debtCode, nameof(debtCode));

            if (!string.IsNullOrWhiteSpace(id))
                UpdateTermTermById(id, debtCode, termType, active);
            else
                AddNewTermDetails(debtCode, termType, communicationType, internalHost, linkToAccess, linkToAccessType,
                    typeOfResponse,
                    requiredLogin, authenticationLogin, authenticationPassword, hostKeyFingerPrint,
                    authenticationCodeApp, pathToOriginFile, pathToDestinationFile,
                    pathToFileBackupAtClient, pathToFileBackupAtHostServer, fileDeLimiter,
                    hashearder, fileProtectedPassword,
                    fileHeaderColumns, availableFieldsColumns);
        }

        private void AddNewTermDetails(int debtCode, TermsType termType, FileAccessType communicationType,
            string internalHost,
            string linkToAccess, string linkToAccessType, string typeOfResponse,
            bool requiredLogin, string authenticationLogin, string authenticationPassword,
            string hostKeyFingerPrint, string authenticationCodeApp, string pathToOriginFile,
            string pathToDestinationFile, string pathToFileBackupAtClient,
            string pathToFileBackupAtHostServer, string fileDeLimiter,
            bool hashearder, string fileProtectedPassword,
            IList<string> fileHeaderColumns, IList<string> availableFieldsColumns)
        {
            var fact = new TermDetail(debtCode, termType, this);

            TermDetails.Add(fact);

            fact.AddDetailConfig(null, communicationType, internalHost, linkToAccess, linkToAccessType, typeOfResponse,
                requiredLogin, authenticationLogin, authenticationPassword, hostKeyFingerPrint,
                authenticationCodeApp, pathToOriginFile, pathToDestinationFile,
                pathToFileBackupAtClient, pathToFileBackupAtHostServer, fileDeLimiter,
                hashearder, fileProtectedPassword,
                fileHeaderColumns, availableFieldsColumns);
        }


        public void UpdateTermTermById(string id, int debtCode, TermsType term, bool active)
        {
            ITermDetail termdet = null;
            var foundIt = false;

            if (!string.IsNullOrEmpty(id))
                termdet = TermDetails.FirstOrDefault(temp => temp.Id.Equals(id)
                                                             && temp.Term.Id == Id
                                                             && temp.Term.OrgCode == OrgCode);

            if (termdet == null)
            {
                termdet = new TermDetail(debtCode, term, this);
            }
            else
            {
                foundIt = true;
                //termdet.UpdatedDate = DateTime.UtcNow;
                //termdet.ModifiedBy = "System-- need change for user";
                termdet.Active = active;
                termdet.Deleted = !active;

                var config = termdet.TermDetailConfigs.FirstOrDefault(temp => temp.TermDetailId == termdet.Id);
                if (config != null)
                {
                    config.Active = termdet.Active;
                    config.Deleted = termdet.Deleted;

                    termdet.UpdateTermConfiguration(config.Id,
                        config.CommunicationType,
                        config.InternalHost,
                        config.LinkToAccess,
                        config.LinkToAccessType,
                        config.TypeOfResponse,
                        config.RequiredLogin,
                        config.AuthenticationLogin,
                        Encoding.ASCII.GetString(config.AuthenticationPassword),
                        Encoding.ASCII.GetString(config.HostKeyFingerPrint),
                        config.AuthenticationCodeApp,
                        config.PathToOriginFile,
                        config.PathToDestinationFile,
                        config.PathToFileBackupAtClient,
                        config.PathToFileBackupAtHostServer,
                        config.FileDelimiter,
                        config.HasHeader,
                        config.FileProtectedPassword,
                        config.FileHeaderColumns,
                        config.AvailableFieldsColumns,
                        config.Active);
                }
            }

            if (foundIt == false) TermDetails.Add(termdet);
        }
    }
}