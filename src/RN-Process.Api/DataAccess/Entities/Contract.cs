using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RN_Process.DataAccess;
using RN_Process.Shared.Commun;

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
        public string OrganizationId { get; private set; }

        public virtual ICollection<ContractDetailConfig> ContractDetailConfigs
        {
            get { return _configMapping ??= new List<ContractDetailConfig>(); }
            set => _configMapping = value;
        }

        private void SetCustomer(Organization organization)
        {
            Guard.Against.Null(organization, nameof(organization));
            OrganizationId = organization.Id;
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
    }
}