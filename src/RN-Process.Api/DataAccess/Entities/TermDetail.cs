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
    public class TermDetail : AuditableEntity<string>
    {
        [BsonIgnore] private ICollection<TermDetailConfig> _termConfig;

        public TermDetail(int debtCode, TermsType name, Term term)
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

        public Term Term { get; set; }

        public virtual ICollection<TermDetailConfig> TermDetailConfigs
        {
            get { return _termConfig ??= new List<TermDetailConfig>(); }
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

        private void SetTerm(Term term)
        {
            Guard.Against.Null(term, nameof(term));
            Term = term;
            TermId = term.Id;
            OrgCode = term.OrgCode;
        }


        public void AddDetailConfig(TermDetailConfig termConfig, bool active = true)
        {
            if (!string.IsNullOrWhiteSpace(termConfig.Id))
                UpdateTermConfiguration(termConfig, active);
            else
                AddNewTermDetailConfig(termConfig);
        }

        public void AddNewTermDetailConfig(TermDetailConfig config)
        {
            var fact = CreateConfiguration(config);

            TermDetailConfigs.Add(fact);
        }

   
        public void UpdateTermConfiguration(TermDetailConfig termsDetailConfig, bool active = true)
        {
            TermDetailConfig config = null;
            var foundIt = false;

            if (!string.IsNullOrEmpty(termsDetailConfig.Id))
                config = TermDetailConfigs.FirstOrDefault(temp => temp.Id.Equals(termsDetailConfig.Id)
                                                                  && temp.TermDetailId == Id
                                                                  && temp.TermDetail.OrgCode == OrgCode);

            if (config == null)
            {
                config = CreateConfiguration(termsDetailConfig);
            }
            else
            {
                foundIt = true;
                config.ModifiedDate = DateTime.UtcNow;
                config.ModifiedBy = "System-- need change for user";
                config.Active = active;
                config.Deleted = !active;
            }

            if (foundIt == false) TermDetailConfigs.Add(config);
        }

        private TermDetailConfig CreateConfiguration(TermDetailConfig config)
        {
            var password = Encoding.ASCII.GetString(config.AuthenticationPassword);
            var fingerPrint = Encoding.ASCII.GetString(config.HostKeyFingerPrint);

            var fact = new TermDetailConfig(
                ObjectId.GenerateNewId().ToString(), 
                this,
                config.CommunicationType,
                config.InternalHost,
                config.LinkToAccess,
                config.LinkToAccessType,
                config.TypeOfResponse,
                config.RequiredLogin,
                config.AuthenticationLogin,
                password,
                fingerPrint,
                config.AuthenticationCodeApp,
                config.PathToOriginFile,
                config.PathToDestinationFile,
                config.PathToFileBackupAtClient,
                config.PathToFileBackupAtHostServer,
                config.FileDelimiter,
                config.FileHeaderColumns,
                config.AvailableFieldsColumns);
            return fact;
        }


    }
}