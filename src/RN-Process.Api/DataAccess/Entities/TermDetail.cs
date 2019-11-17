using System;
using System.Collections.Generic;
using System.Linq;
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


        /// <summary>
        ///     Add or update term
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fileType"></param>
        /// <param name="active"></param>
        /// <param name="deleted"></param>
        public void AddDetailConfig(string id, FileAccessType fileType, bool active = true, bool deleted = false)
        {
            if (!string.IsNullOrWhiteSpace(id))
                UpdateTermConfigurationById(id, fileType, active, deleted);
            else
                AddNewTermConfiguration(fileType);
        }

        /// <summary>
        ///     Add new configuration
        /// </summary>
        /// <param name="fileType"></param>
        private void AddNewTermConfiguration(FileAccessType fileType)
        {
            var fact = new TermDetailConfig(this, fileType, 
                string.Empty,
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), 
                string.Empty, 
                string.Empty,
                string.Empty, 
                false, 
                string.Empty, 
                string.Empty, "", 
                string.Empty, 
                string.Empty, 
                string.Empty, 
                string.Empty,
                string.Empty, 
                string.Empty, 
                new List<string> { RnProcessConstant.ColumnsBaseIntrum },
                new List<string> { RnProcessConstant.ColumnsBaseClient });

            TermDetailConfigs.Add(fact);
        }


        public void UpdateTermConfigurationById(string id, FileAccessType fileType, bool active = true,
            bool deleted = false)
        {
            TermDetailConfig config = null;
            var foundIt = false;

            if (!string.IsNullOrEmpty(id))
                config = TermDetailConfigs.FirstOrDefault(temp => temp.Id.Equals(id)
                                                                      && temp.TermDetailId == Id
                                                                      && temp.TermDetail.OrgCode == OrgCode);

            if (config == null)
            {
                config = new TermDetailConfig(this, fileType,
                    string.Empty,
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    false,
                    string.Empty,
                    string.Empty, "",
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    new List<string> { RnProcessConstant.ColumnsBaseIntrum },
                    new List<string> { RnProcessConstant.ColumnsBaseClient });
            }
            else
            {
                foundIt = true;
                config.ModifiedDate = DateTime.UtcNow;
                config.ModifiedBy = "System-- need change for user";
                config.Active = active;
                config.Deleted = deleted;
            }

            if (foundIt == false) TermDetailConfigs.Add(config);
        }

    }
}