using System;
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
            DueDetailConfigs = new List<DueDetailConfiguration>();
        }

        public string Id { get; set; }

        /// <summary>
        ///     Cod organization
        /// </summary>
        public string CodOrg { get; set; }

        /// <summary>
        ///     Name that describe the organization
        /// </summary>
        public string Description { get; set; }

        public int ContractNumber { get; set; }

        public string DueId { get; set; }

        [JsonIgnore]
        public bool IsDeleted { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? ChangedDate { get; set; }

        public List<DueDetail> DueDetails { get; set; }
        public List<DueDetailConfiguration> DueDetailConfigs { get; set; }
      


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}