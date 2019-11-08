using RN_Process.DataAcces;
using System;
using System.Collections.Generic;
using System.Text;
using RN_Process.DataAccess;

namespace RN_Process.Api.Models
{
    public class FileImport : Entity<Guid>
    {
        public FileImport()
        {
            ContractMappingBase = new ContractMappingBase();
        }

        public virtual ContractMappingBase ContractMappingBase { get; set; }
    }
}
