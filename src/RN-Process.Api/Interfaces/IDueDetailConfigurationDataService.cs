using System.Collections.Generic;
using System.Threading.Tasks;
using RN_Process.Api.Models;

namespace RN_Process.Api.Interfaces
{
    public interface IDueDetailDataService
    {
        Task<DueDetail> GetDueDetailConfigurationById(string id);
        Task<IList<DueDetail>> GetDueDetailConfigurationOrgCod(string codorg);
        Task Create(DueDetail fileContract);
    }
}