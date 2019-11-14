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
    [BsonKnownTypes(typeof(Organization))]
    public class Organization : AuditableEntity<string>
    {
        private static readonly DateTime DefaultDateTime = DateTime.UtcNow;

        [BsonIgnore] private ICollection<Contract> _contract;

        [BsonIgnore] private ICollection<ContractDetailConfig> _contractConfig;

        public Organization(string description, string orgCode)
        {
            Id = ObjectId.GenerateNewId().ToString();
            SetDescription(description);
            SetOrgCode(orgCode);
            SetVersion();

            Active = true;
            Deleted = false;
            RowVersion = new byte[0];
        }

        /// <summary>
        /// </summary>
        protected Organization()
        {
        }

        public virtual string OrgCode { get; private set; }
        public string Description { get; private set; }
        public string Uri { get; set; }

        public virtual ICollection<Contract> Contracts
        {
            get { return _contract ??= new List<Contract>(); }
            protected set => _contract = value;
        }

        public virtual ICollection<ContractDetailConfig> ContractDetails
        {
            get { return _contractConfig ??= new List<ContractDetailConfig>(); }
            protected set => _contractConfig = value;
        }

        /// <summary>
        /// </summary>
        /// <param name="orgCode"></param>
        private void SetOrgCode(string orgCode)
        {
            Guard.Against.NullOrEmpty(orgCode, nameof(orgCode));
            Guard.Against.NullOrWhiteSpace(orgCode, nameof(orgCode));
            Guard.Against.OutOfRange(orgCode.Length, nameof(orgCode), 3, 10);

            OrgCode = orgCode.ToUpper();
        }

        /// <summary>
        /// </summary>
        /// <param name="description"></param>
        private void SetDescription(string description)
        {
            Guard.Against.NullOrEmpty(description, nameof(description));
            Guard.Against.OutOfRange(description.Length, nameof(description), 5, 250);
            Description = description;
        }

        /// <summary>
        ///     Return a version byte
        /// </summary>
        private void SetVersion()
        {
            RowVersion = new byte[0];
        }


        /// <summary>
        /// </summary>
        /// <param name="contractNumber"></param>
        /// <param name="typeDebt"></param>
        /// <param name="debtDescription"></param>
        public void AddNewContract(int contractNumber, int typeDebt, string debtDescription)
        {
            //create contract
            var fact = new Contract(contractNumber, typeDebt, debtDescription, this);

            //base configuration
            fact.AddContraDetailConfig(null, FileAccessType.FTP, true, false);

            //add contract to list contract list of organization
            Contracts.Add(fact);

            //add contract configuration details to list contract configuration details list of organization
            ContractDetails = fact.ContractDetailsConfigs;
        }


        /// <summary>
        ///     Add contract
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contractNumber"></param>
        /// <param name="typeDebt"></param>
        /// <param name="debtDescription"></param>
        public void AddContract(string id, int contractNumber, int typeDebt, string debtDescription)
        {
            Guard.Against.Null(contractNumber, nameof(contractNumber));
            Guard.Against.Zero(contractNumber, nameof(contractNumber));
            Guard.Against.Null(typeDebt, nameof(typeDebt));
            Guard.Against.Zero(typeDebt, nameof(typeDebt));

            if (!string.IsNullOrEmpty(id))
                UpdateExistingContractById(id, contractNumber, typeDebt, debtDescription);
            else
                AddNewContract(contractNumber, typeDebt, debtDescription);
        }

        /// <summary>
        ///     Update existing contract by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contractNumber"></param>
        /// <param name="typeDebt"></param>
        /// <param name="debtDescription"></param>
        private void UpdateExistingContractById(string id,
            int contractNumber,
            int typeDebt,
            string debtDescription,
            bool active = true,
            bool deleted = false)
        {
            Contract contract = null;
            var foundIt = false;

            if (!string.IsNullOrEmpty(id)) contract = Contracts.Where(temp => temp.Id == id).FirstOrDefault();
            //for 
            if (contract == null)
            {
                contract = new Contract(contractNumber, typeDebt, debtDescription, this);
                contract.AddContraDetailConfig(null, FileAccessType.FTP, true, false);
            }
            else
            {
                //add update contract ?
                foundIt = true;
                contract.ModifiedDate = DateTime.UtcNow;
                contract.ModifiedBy = "System-- need change for user";
                contract.Active = active;
                contract.Deleted = deleted;

                var config = contract.ContractDetailsConfigs.Where(temp => temp.ContractId == contract.Id);
                foreach (var item in config)
                    contract.UpdateContractConfigurationById(item.Id, item.CommunicationType, active, deleted);
            }

            if (foundIt == false) Contracts.Add(contract);
        }

        public void RemoveContract(string id, bool softDelete)
        {
            if (string.IsNullOrEmpty(id)) return;

            var match = Contracts.FirstOrDefault(fact => fact.Id == id);

            if (match == null) return;

            if (!softDelete)
                Contracts.Remove(match);
            else
                UpdateExistingContractById(match.Id, match.ContractNumber, match.TypeDebt, match.DebtDescription, false,
                    true);
        }
    }
}