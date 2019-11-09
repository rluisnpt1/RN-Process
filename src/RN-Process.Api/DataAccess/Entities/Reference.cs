
using System;
using System.Collections.Generic;
using System.Text;
using RN_Process.DataAccess;

namespace RN_Process.Api.DataAccess.Entities
{
    public class Reference :  Entity<string>
    {
        public string UniqCode { get; set; }

        public string ReferencesTypeId { get; set; }
        public virtual ReferencesType ReferencesType  { get; set; }
    }
}
