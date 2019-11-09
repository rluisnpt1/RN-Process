using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using RN_Process.DataAccess;
using RN_Process.Shared.Commun;

namespace RN_Process.Api.Models
{
    public class Customer : EntityMdb<string>
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual string UniqCode { get; private set; }

        /// <summary>
        /// The Name value is a string that can store the a string that can store the description .
        /// </summary>
        [StringLength(250)]
        public string Description { get;  private set; }


        public Customer(string description, string uniqCode)
        {
            Id = ObjectId.GenerateNewId().ToString();
            SetDescription(description);
            SetUniqCode(uniqCode);
            SetVersion();
            CreatedBy = string.Empty;
            ModifiedBy = string.Empty;
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
            Guard.Against.NullOrEmpty(uniqCode,nameof(uniqCode));
            Guard.Against.OutOfRange(uniqCode.Length, nameof(uniqCode), 3, 10);
            UniqCode = uniqCode;
        }

        private void SetVersion()
        {
            RowVersion = new byte[0];
        }


    }
}
