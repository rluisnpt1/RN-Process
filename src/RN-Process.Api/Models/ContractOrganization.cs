﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
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
        [JsonIgnore]
        public bool IsDeleted { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? ChangedDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<DueDetail> DueDetails { get; set; }

        public void AddDueDetail(int debtCode, TermsType termsType)
        {
            DueDetails.Add(new DueDetail
            {
                DebtCode = debtCode,
                TermsType = termsType
            });
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}