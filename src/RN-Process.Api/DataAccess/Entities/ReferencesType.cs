using System.Collections.Generic;
using RN_Process.DataAccess;

namespace RN_Process.Api.DataAccess.Entities
{
    public class ReferencesType : Entity<string>
    {
        public virtual List<Reference> References { get; set; }
        public string UniqCode { get; set; }

        public ReferencesType(string uniqCode)
        {
            References =new List<Reference>();
            UniqCode = uniqCode;    
        }
    }
}
