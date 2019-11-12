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
        public int ContractNumber { get; private set; }

        public int TypeDebt { get; private set; }

        public string DebtDescription { get; private set; }

        public string OrganizationId { get; private set; }
        public virtual Organization Organization { get; set; }


        [BsonIgnore]
        private ICollection<ContractDetailConfig> _configMapping;

        public virtual ICollection<ContractDetailConfig> ContractDetailConfigs
        {
            get { return _configMapping ??= new List<ContractDetailConfig>(); }
            set => _configMapping = value;
        }

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
