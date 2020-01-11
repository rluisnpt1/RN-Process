using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Api.Models;

namespace RN_Process.Api.Interfaces
{
    public interface IContractFileDataService
    {
        Task<FileDataContract> GetOrganizationFileById(string id);
        Task<IList<FileDataContract>> GetOrganizationFileByOrgCod(string codorg);
        Task CreateOrganizationFile(FileDataContract fileContract);
    }
}
