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
        [BsonIgnore] private ICollection<TermDetail> _termDetail;

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


        public virtual ICollection<TermDetail> TermDetails
        {
            get { return _termDetail ??= new List<TermDetail>(); }
            protected set => _termDetail = value;
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
        public void AddNewContract(int contractNumber, int typeDebt)
        {
            //create contract
            var fact = new Contract(contractNumber, this);

            //add contract to list contract list of organization
            Contracts.Add(fact);

            //base configuration
            fact.AddTerm(null, typeDebt, TermsType.Loan, true, false);

            //add contract configuration details to list contract configuration details list of organization
            TermDetails = fact.TermDetails;
        }


        /// <summary>
        ///     Add contract
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contractNumber"></param>
        /// <param name="typeDebt"></param>
        /// <param name="debtDescription"></param>
        public void AddContract(string id, int contractNumber, int typeDebt)
        {
            Guard.Against.Null(contractNumber, nameof(contractNumber));
            Guard.Against.Zero(contractNumber, nameof(contractNumber));
            Guard.Against.Null(typeDebt, nameof(typeDebt));
            Guard.Against.Zero(typeDebt, nameof(typeDebt));

            if (!string.IsNullOrEmpty(id))
                UpdateExistingContractById(id, typeDebt, contractNumber);
            else
                AddNewContract(contractNumber, typeDebt);
        }

       
        private void UpdateExistingContractById(string id, int debtCode, int contractNumber, bool active = true, bool deleted = false)
        {
            Contract contract = null;
            var foundIt = false;

            if (!string.IsNullOrEmpty(id)) contract = Contracts.FirstOrDefault(temp => temp.Id == id);
            //for 
            if (contract == null)
            {
                contract = new Contract(contractNumber, this);
                contract.AddTerm(null,debtCode, TermsType.Loan);
            }
            else
            {
                //add update contract ?
                foundIt = true;
                contract.ModifiedDate = DateTime.UtcNow;
                contract.ModifiedBy = "System-- need change for user";
                contract.Active = active;
                contract.Deleted = deleted;

                var config = contract.TermDetails.Where(temp => temp.ContractId == contract.Id);
                foreach (var item in config)
                    contract.UpdateContractTermById(item.Id, item.DebtCode, item.TermsType, active, deleted);
            }

            if (foundIt == false) Contracts.Add(contract);
        }

        public void RemoveContracts(string id)//, bool softDelete)
        {
            if (string.IsNullOrEmpty(id)) return;

            var matchContract = Contracts.FirstOrDefault(fact => fact.Id == id);

            if (matchContract == null) return;

            var match = matchContract.TermDetails.FirstOrDefault(x => x.Contract.Id == matchContract.Id);

            //if (!softDelete)
            //    Contracts.Remove(matchContract);
            //else
            
            if (match == null) return;

            UpdateExistingContractById(matchContract.Id, match.DebtCode, matchContract.ContractNumber, false, true);
        }
    }
}