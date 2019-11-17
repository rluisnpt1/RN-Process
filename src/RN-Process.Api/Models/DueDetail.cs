using System.ComponentModel.DataAnnotations;
using RN_Process.Shared.Enums;

namespace RN_Process.Api.Models
{
    public class DueDetail
    {
        public string Id { get; set; }
        /// <summary>
        /// Uniq code that identify the debt
        /// </summary>
        [Display(Name = "Debt Code (Intrum)")]
        [Required]
        public int DebtCode { get; set; }
        /// <summary>
        /// Description of debt
        /// </summary>
        [Display(Name = "Type Debt")]
        [Required]
        public TermsType TermsType { get; set; }

        public DueDetail()
        {
        }



    }
}