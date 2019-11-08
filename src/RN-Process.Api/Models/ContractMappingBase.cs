using RN_Process.DataAcces;
using System;
using System.Collections.Generic;
using System.Text;
using RN_Process.DataAccess;

namespace RN_Process.Api.Models
{
    public class ContractMappingBase : Entity<Guid>
    {
        public ContractMappingBase()
        {
            Contract = new Contract();
        }

        public virtual Contract Contract { get; set; }
    }
}
