using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using RN_Process.Shared.Commun;
using RN_Process.Shared.Enums;

namespace RN_Process.Api.Models
{
    public class Due
    {
        public Due()
        {
            FileHeaderColumns = new List<string>() { "CLIENT_COLUM1", "CLIENT_COLUM2", "CLIENT_COLUM3" };
            AvailableFieldsColumns = RnProcessConstant.AvailableColumnsIntrum.ToList();
        }

        
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

        /// <summary>
        /// Type communication has been agreed 
        /// </summary>
        [Required]
        public FileAccessType CommunicationType { get; set; }

        /// <summary>
        /// link or path to access the file
        /// </summary>
        [Display(Name = "Address to Access file (LINK|IP)")]
        [Required]
        public string LinkToAccess { get; set; }

        /// <summary>
        /// Type acces //SFTP HTTP HTTPS
        /// </summary>
        [Display(Name = "Type Access")]
        public string LinkToAccessType { get; set; }

        /// <summary>
        /// File Format Agreed
        /// </summary>
        [Display(Name = "Type File Response")]
        public string TypeOfResponse { get; set; }

        /// <summary>
        /// If need login
        /// </summary>
        [Display(Name = "Required Login?")]
        public bool RequiredLogin { get; set; }

        [Display(Name = "User Login")]
        public string AuthenticationLogin { get; set; }

        [Display(Name = "Password")]
        public byte[] AuthenticationPassword { get; set; }


        [Display(Name = "SSH Host key Finger Print ")]
        public string HostkeyFingerPrint { get; set; }


        [Display(Name = "app code identification")]
        public string AuthenticationCodeApp { get; set; }


        [Display(Name = "Path to get file (client side)")]
        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PathToOriginFile { get; set; }

        [Display(Name = "Path to send file back (client side)")]
        public string PathToDestinationFile { get; set; }

        [Display(Name = "Path to File backup (client side)")]
        public string PathToFileBackupAtClient { get; set; }

        [Display(Name = "File Delimiter")]
        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FileDelimiter { get; set; }

        [Display(Name = "Columns Names at file")]
        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public IList<string> FileHeaderColumns { get; set; }


        [Display(Name = "Intrum Columns Available")]
        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public IList<string> AvailableFieldsColumns { get; set; }

        [JsonIgnore]
        public bool IsDeleted { get; set; }



    }
}