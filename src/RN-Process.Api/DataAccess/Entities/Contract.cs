using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RN_Process.DataAccess;
using RN_Process.Shared.Commun;

namespace RN_Process.Api.DataAccess.Entities
{
    public class Contract : Entity<string>
    {
        public int ContractNumber { get; private set; }

        public int TypeDebt { get; private set; }

        public string DebtDescription { get; private set; }
        
        public virtual ICollection<ContractMappingBase> ContractMappingBases { get; set; }


        [BsonRepresentation(BsonType.ObjectId)]
        public string CustomerId { get; private set; }
        public virtual Customer Customer { get; set; }

        //Runtime execution
        protected Contract()
        {
            
        }

        public Contract(int contractNumber, int typeDebt, string debtDescription, Customer customer)
        {
            SetContractNumber(contractNumber);
            SetTypeDebt(typeDebt);
            SetCustomer(customer);
            DebtDescription = debtDescription;
            CreatedBy = string.Empty;
            ModifiedBy = string.Empty;
            ContractMappingBases = new List<ContractMappingBase>();
        }

        private void SetCustomer(Customer customer)
        {
            Guard.Against.Null(customer, nameof(customer));
            CustomerId = CustomerId;
            Customer = customer;
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
