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
    public class Contract : AuditableEntity<string>
    {
        [BsonIgnore] private ICollection<ContractDetailConfig> _configMapping;


        //Runtime execution
        protected Contract()
        {
        }


        public Contract(int contractNumber, int typeDebt, string debtDescription, Organization organization)
        {
            Id = ObjectId.GenerateNewId().ToString();
            SetContractNumber(contractNumber);
            SetTypeDebt(typeDebt);
            SetCustomer(organization);
            Active = true;
            Deleted = false;
            DebtDescription = debtDescription;
            CreatedDate = DateTime.UtcNow;
        }

        public virtual Organization Organization { get; set; }


        public int ContractNumber { get; private set; }
        public int TypeDebt { get; private set; }
        public string DebtDescription { get; }

        public virtual string OrgCode { get; private set; }
        public string OrganizationId { get; private set; }

   
        public virtual ICollection<ContractDetailConfig> ContractDetailsConfigs
        {
            get { return _configMapping ??= new List<ContractDetailConfig>(); }
            protected set => _configMapping = value;
        }

        private void SetCustomer(Organization organization)
        {
            Guard.Against.Null(organization, nameof(organization));
            OrganizationId = organization.Id;
            OrgCode = organization.OrgCode;
            Organization = organization;
        }

        private void SetTypeDebt(int typeDebt)
        {
            Guard.Against.Zero(typeDebt, nameof(typeDebt));
            TypeDebt = typeDebt;
        }

        private void SetContractNumber(int contractNumber)
        {
            Guard.Against.Zero(contractNumber, nameof(ContractNumber));
            ContractNumber = contractNumber;
        }


        /// <summary>
        /// Add or update contract
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fileType"></param>
        /// <param name="active"></param>
        /// <param name="deleted"></param>
        public void AddContraDetailConfig(string id, FileAccessType fileType, bool active, bool deleted)
        {
            if (!string.IsNullOrEmpty(id))
                UpdateContractConfigurationById(id, fileType, active, deleted);
            else
                AddNewContractConfiguration(fileType);
        }

        /// <summary>
        /// Add new configuration
        /// </summary>
        /// <param name="fileType"></param>
        private void AddNewContractConfiguration(FileAccessType fileType)
        {
            var fact = ContractAddNewEmptyContractConfiguratiom(fileType);
            ContractDetailsConfigs.Add(fact);
        }


        /// <summary>
        /// </summary>
        /// <param name="contr"></param>
        public void UpdateContractConfigurationById(string id, FileAccessType fileType, bool active = true, bool deleted = false)
        {

            ContractDetailConfig config = null;
            var foundIt = false;

            if (!string.IsNullOrEmpty(id))
            {
                config = ContractDetailsConfigs.FirstOrDefault(temp => temp.Id.Equals(id)
                                                           && temp.Contract.Id == this.Id
                                                           && temp.Contract.OrgCode == OrgCode);
            }

            if (config == null)
            {
                config = ContractAddNewEmptyContractConfiguratiom(fileType);
            }
            else
            {
                foundIt = true;
                config.ModifiedDate = DateTime.UtcNow;
                config.ModifiedBy = "System-- need change for user";
                config.Active = active;
                config.Deleted = deleted;
            }

            if (foundIt == false) ContractDetailsConfigs.Add(config);
        }

        /// <summary>
        /// Createan empty contract configuration when new contract is create
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        private ContractDetailConfig ContractAddNewEmptyContractConfiguratiom(FileAccessType fileType)
        {
            return new ContractDetailConfig(fileType,
                    string.Empty, string.Empty, string.Empty,
                    string.Empty, false, string.Empty,
                    string.Empty, string.Empty, string.Empty,
                    string.Empty, string.Empty, string.Empty,
                    string.Empty,
                    new List<string>() { RnProcessConstant.ColumnsBaseIntrum },
                    new List<string>() { RnProcessConstant.ColumnsBaseClient },
                    this);
        }

    }
}