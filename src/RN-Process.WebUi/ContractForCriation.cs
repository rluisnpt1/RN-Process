using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RN_Process.Shared.Enums;

namespace RN_Process.WebUi
{
    public class ContractForCriation
    {

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


        [Display(Name = "Debt Code (Intrum)")]
        [Required]
        public int DebtCode { get; set; }

        /// <summary>
        ///     Description of debt
        /// </summary>
        [Display(Name = "Type Debt")]
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public string TermsType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
