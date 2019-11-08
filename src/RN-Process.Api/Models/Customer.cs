using RN_Process.DataAcces;
using System;
using System.Collections.Generic;
using System.Text;
using RN_Process.DataAccess;

namespace RN_Process.Api.Models
{
    public class Customer : Entity<Guid>
    {
        public string Description { get; set; }
        public string UNICode { get; set; }
    }
}
