using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RN_Process.DataAccess;
using RN_Process.Shared.Commun;

namespace RN_Process.Api.DataAccess.Entities
{
    [BsonKnownTypes(typeof(Organization))]
    public class Organization : AuditableEntity<string>
    {
        private static readonly DateTime DefaultDateTime = DateTime.UtcNow;

        [BsonIgnore] private ICollection<Contract> _contract;

        [BsonIgnore] private ICollection<ContractDetailConfig> _contractConfig;

        public Organization(string description, string uniqCode)
        {
            Id = ObjectId.GenerateNewId().ToString();
            SetDescription(description);
            SetUniqCode(uniqCode);
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

        public virtual string UniqCode { get; private set; }
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

        private void SetDescription(string description)
        {
            Guard.Against.NullOrEmpty(description, nameof(description));
            Guard.Against.OutOfRange(description.Length, nameof(description), 5, 250);
            Description = description;
        }


        private void SetUniqCode(string uniqCode)
        {
            Guard.Against.NullOrEmpty(uniqCode, nameof(uniqCode));
            Guard.Against.OutOfRange(uniqCode.Length, nameof(uniqCode), 3, 10);

            UniqCode = uniqCode;
        }

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
            var fact = new Contract(contractNumber, typeDebt, debtDescription, this);

            UpdateContractConfigurationById(fact);

            Contracts.Add(fact);
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
        /// <param name="modificationDate"></param>
        private void UpdateExistingContractById(string id, int contractNumber, int typeDebt, string debtDescription)
        {
            var foundIt = false;

            // locate existing contract 
            var contract = Contracts
                .Where(temp => temp.Id == id && temp.ContractNumber == contractNumber && temp.TypeDebt == typeDebt)
                .FirstOrDefault();

            if (contract == null)
            {
                contract = new Contract(contractNumber, typeDebt, debtDescription, this);
                //Id = ObjectId.GenerateNewId().ToString();
            }
            else
            {
                foundIt = true;
                contract.ModifiedDate = DateTime.UtcNow;
                contract.ModifiedBy = "System-- need change for user";
            }

            if (foundIt == false) Contracts.Add(contract);
        }

        /// <summary>
        /// </summary>
        /// <param name="contr"></param>
        public void UpdateContractConfigurationById(Contract contr)
        {
            var foundIt = false;

            var contractDetail = ContractDetails
                .Where(temp => temp.ContractId == contr.Id
                               && temp.Contract.OrganizationId == Id)
                .FirstOrDefault();

            if (contractDetail == null)
            {
                contractDetail = new ContractDetailConfig(string.Empty,
                    string.Empty, string.Empty, string.Empty,
                    string.Empty, false, string.Empty,
                    string.Empty, string.Empty, string.Empty,
                    string.Empty, string.Empty, string.Empty,
                    string.Empty, new List<string>(), new List<string>(), contr);
            }
            else
            {
                foundIt = true;
                contractDetail.ModifiedDate = DateTime.UtcNow;
                contractDetail.ModifiedBy = "System-- need change for user";
            }

            if (foundIt == false) ContractDetails.Add(contractDetail);
        }
    }
}