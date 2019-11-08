using RN_Process.DataAcces;
using System;
using System.Collections.Generic;
using System.Text;
using RN_Process.DataAccess;

namespace RN_Process.Api.Models
{
    public class Contract : Entity<Guid>
    {
        public Contract()
        {
            Customer = new Customer();
        }
        public virtual Customer Customer { get; set; }
    }
}
