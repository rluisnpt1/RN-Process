using System.Collections.Generic;
using System.Threading.Tasks;
using RN_Process.Api.Models;

namespace RN_Process.Api.Interfaces
{
    public interface IDueDetailConfigurationDataService
    {
        Task<DueDetailConfiguration> GetDueDetailConfigurationById(string id);
        Task<IList<DueDetailConfiguration>> GetDueDetailConfigurationOrgCod(string codorg);
        Task Create(DueDetailConfiguration fileContract);
    }
}