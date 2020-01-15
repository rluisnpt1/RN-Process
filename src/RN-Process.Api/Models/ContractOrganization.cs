using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RN_Process.Shared.Enums;

namespace RN_Process.Api.Models
{
 

    public class ContractOrganization
    {
        public ContractOrganization()
        {
            CodOrg = string.Empty;
            Description = string.Empty;
            DueDetails = new List<DueDetail>();
        }

        public ContractOrganization(string codOrg,string description, int contractNumber)
        {
            CodOrg = codOrg;
            Description = description;
            ContractNumber = contractNumber;
        }

        public string Id { get; set; }

        /// <summary>
        ///     Cod organization
        /// </summary>
        [Display(Name = "Organization Code (Intrum)")]
        [Required]
        public string CodOrg { get; set; }

        /// <summary>
        ///     Name that describe the organization
        /// </summary>
        [Display(Name = "Description")]
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Contract Number")]
        [Required]
        public int ContractNumber { get; set; }

        public string DueId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //[JsonIgnore]
        public bool IsDeleted { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public List<DueDetail> DueDetails { get; set; }

        public void AddDueDetail(int debtCode, string termsType)
        {
            DueDetails = new List<DueDetail>
            {
                new DueDetail(null, debtCode,termsType)
            };

        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}