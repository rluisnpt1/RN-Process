
using System;
using System.Collections.Generic;
using System.Text;
using RN_Process.DataAccess;

namespace RN_Process.Api.Models
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
