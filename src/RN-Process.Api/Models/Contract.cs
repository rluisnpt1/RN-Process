
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using MongoDB.Bson;
using RN_Process.DataAccess;
using RN_Process.Shared.Commun;
using RN_Process.Shared.Enums;

namespace RN_Process.Api.Models
{
    public class Contract : EntityMdb<string>
    {
        public int ContractNumber { get; private set; }

        public int TypeDebt { get; private set; }

        public string DebtDescription { get; private set; }

        public Contract(int contractNumber, int typeDebt, string debtDescription, Customer customer)
        {
            Id = ObjectId.GenerateNewId().ToString();
            SetContractNumber(contractNumber);
            SetTypeDebt(typeDebt);

            DebtDescription = debtDescription;
            CreatedBy = string.Empty;
            ModifiedBy = string.Empty;
            Customer = customer;
        }

        private void SetRequisitionType(RequisitionType requisitionType)
        {
            string enumDesc = requisitionType.DescriptionAttr();
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

        public virtual Customer Customer { get; set; }
    }
}
