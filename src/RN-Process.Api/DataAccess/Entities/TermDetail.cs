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
        [BsonIgnore] private ICollection<ContractDetailConfig> _contractConfig;

        public TermDetail(int debtCode, TermsType name, Contract contract)
        {
            Id = ObjectId.GenerateNewId().ToString();
            SetDebtCode(debtCode);
            SerTermTypeName(name);
            SetContract(contract);
            Active = true;
            Deleted = false;
            RowVersion = new byte[0];
        }

        public int DebtCode { get; private set; }
        public TermsType TermsType { get; private set; }
        public string OrgCode { get; private set; }

        public string ContractId { get; private set; }

        public Contract Contract { get; set; }

        public virtual ICollection<ContractDetailConfig> ContractDetailConfigs
        {
            get { return _contractConfig ??= new List<ContractDetailConfig>(); }
            protected set => _contractConfig = value;
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

        private void SetContract(Contract contract)
        {
            Guard.Against.Null(contract, nameof(contract));
            Contract = contract;
            ContractId = contract.Id;
            OrgCode = contract.OrgCode;
        }


        /// <summary>
        ///     Add or update contract
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
            var fact = new ContractDetailConfig(this, fileType, 
                string.Empty,
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), 
                string.Empty, 
                string.Empty,
                string.Empty, 
                false, 
                string.Empty, 
                string.Empty, 
                string.Empty, 
                string.Empty, 
                string.Empty, 
                string.Empty,
                string.Empty, 
                string.Empty, 
                new List<string> { RnProcessConstant.ColumnsBaseIntrum },
                new List<string> { RnProcessConstant.ColumnsBaseClient });

            ContractDetailConfigs.Add(fact);
        }


        public void UpdateTermConfigurationById(string id, FileAccessType fileType, bool active = true,
            bool deleted = false)
        {
            ContractDetailConfig config = null;
            var foundIt = false;

            if (!string.IsNullOrEmpty(id))
                config = ContractDetailConfigs.FirstOrDefault(temp => temp.Id.Equals(id)
                                                                      && temp.TermDetailId == Id
                                                                      && temp.TermDetail.OrgCode == OrgCode);

            if (config == null)
            {
                config = new ContractDetailConfig(this, fileType,
                    string.Empty,
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    false,
                    string.Empty,
                    string.Empty,
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

            if (foundIt == false) ContractDetailConfigs.Add(config);
        }

        

        public void UpdateContractConfig(ContractDetailConfig item)
        {
            throw new NotImplementedException();
        }
    }
}