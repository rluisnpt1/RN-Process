using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using RN_Process.DataAccess;
using RN_Process.Shared.Commun;

namespace RN_Process.Api.DataAccess.Entities
{
    public class Customer : EntityMdb<string>
    {
        private static readonly DateTime DefaultDateValue = DateTime.MinValue;
        /// <summary>
        /// 
        /// </summary>
        public virtual string UniqCode { get; private set; }

        /// <summary>
        /// The Name value is a string that can store the a string that can store the description .
        /// </summary>
        [StringLength(250)]
        public string Description { get; private set; }

        public virtual List<Contract> Contracts { get; set; }

        public Customer(string description, string uniqCode)
        {
            Id = ObjectId.GenerateNewId().ToString();
            SetDescription(description);
            SetUniqCode(uniqCode);
            SetVersion();
            CreatedBy = string.Empty;
            ModifiedBy = string.Empty;
            Contracts = new List<Contract>();
        }

        protected Customer()
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
            var fact = new Contract(contractNumber, typeDebt, debtDescription, this) { Id = id };
            Contracts.Add(fact);
        }

        ///// <summary>
        ///// Add contract 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="contractNumber"></param>
        ///// <param name="typeDebt"></param>
        ///// <param name="debtDescription"></param>
        //public void AddContract(string id, int contractNumber, int typeDebt, string debtDescription)
        //{
        //    Guard.Against.Null(contractNumber, nameof(contractNumber));
        //    Guard.Against.Zero(contractNumber, nameof(contractNumber));
        //    Guard.Against.Null(typeDebt, nameof(typeDebt));
        //    Guard.Against.Zero(typeDebt, nameof(typeDebt));

        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        UpdateExistingContractById(id, contractNumber, typeDebt, debtDescription, DefaultDateValue);
        //    }
        //    else
        //    {
        //        AddNewContract(id, contractNumber, typeDebt, debtDescription);
        //    }
        //}

        ///// <summary>
        ///// Update existing contract by id 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="contractNumber"></param>
        ///// <param name="typeDebt"></param>
        ///// <param name="debtDescription"></param>
        ///// <param name="modificationDate"></param>
        //private void UpdateExistingContractById(string id, int contractNumber, int typeDebt, string debtDescription, DateTime modificationDate)
        //{
        //    bool foundIt = false;

        //    // locate existing contract 
        //    Contract contract = (from temp in Contracts
        //        where temp.ContractNumber == contractNumber
        //              && temp.TypeDebt == typeDebt
        //        select temp).FirstOrDefault();

        //    if (contract == null)
        //    {
        //        contract = new Contract(contractNumber, typeDebt, debtDescription, this) {Id = id};
        //        //Id = ObjectId.GenerateNewId().ToString();
        //    }
        //    else
        //    {
        //        foundIt = true;
        //        contract.ModifiedDate = modificationDate;
        //        contract.ModifiedBy = "System-- need change for user";
        //    }

        //    if (foundIt == false)
        //    {
        //        Contracts.Add(contract);
        //    }
        //}
    }
}
