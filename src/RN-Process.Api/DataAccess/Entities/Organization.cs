using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public virtual string UniqCode { get; private set; }
        public string Description { get; private set; }
        public string Uri { get; set; }
        
        [BsonIgnore]
        private ICollection<Contract> _contract;
        public virtual ICollection<Contract> Contracts
        {
            get { return _contract ??= new List<Contract>(); }
            protected set => _contract = value;
        }

        public Organization(string description, string uniqCode)
        {
            Id = ObjectId.GenerateNewId().ToString();
            SetDescription(description);
            SetUniqCode(uniqCode);
            SetVersion();
        }

        protected Organization()
        {

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

        public void AddNewContract(string id, int contractNumber, int typeDebt, string debtDescription)
        {
            var fact = new Contract(contractNumber, typeDebt, debtDescription, this);
            Contracts.Add(fact);
        }

        /// <summary>
        /// Add contract 
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
            {
                UpdateExistingContractById(id, contractNumber, typeDebt, debtDescription, DefaultDateTime);
            }
            else
            {
                AddNewContract(id, contractNumber, typeDebt, debtDescription);
            }
        }

        /// <summary>
        /// Update existing contract by id 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contractNumber"></param>
        /// <param name="typeDebt"></param>
        /// <param name="debtDescription"></param>
        /// <param name="modificationDate"></param>
        private void UpdateExistingContractById(string id, int contractNumber, int typeDebt, string debtDescription, DateTime modificationDate)
        {
            bool foundIt = false;

            // locate existing contract 
            Contract contract = (Contracts.Where(temp => temp.ContractNumber == contractNumber && temp.TypeDebt == typeDebt)).FirstOrDefault();

            if (contract == null)
            {
                contract = new Contract(contractNumber, typeDebt, debtDescription, this) { Id = id };
                //Id = ObjectId.GenerateNewId().ToString();
            }
            else
            {
                foundIt = true;
                contract.ModifiedDate = modificationDate;
                contract.ModifiedBy = "System-- need change for user";
            }

            if (foundIt == false)
            {
                Contracts.Add(contract);
            }
        }
    }
}
