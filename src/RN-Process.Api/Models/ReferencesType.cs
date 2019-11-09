
using System;
using System.Collections.Generic;
using System.Text;
using RN_Process.DataAccess;

namespace RN_Process.Api.Models
{
    public class ReferencesType : Entity<Guid>
    {
        public string UniqCode { get; set; }
    }
}
