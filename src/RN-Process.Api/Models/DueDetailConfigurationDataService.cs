using System.Collections.Generic;
using System.Threading.Tasks;
using RN_Process.Api.Interfaces;

namespace RN_Process.Api.Models
{
    public class DueDetailConfigurationDataService: IDueDetailConfigurationDataService
    {
        public Task<DueDetailConfiguration> GetDueDetailConfigurationById(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IList<DueDetailConfiguration>> GetDueDetailConfigurationOrgCod(string codorg)
        {
            throw new System.NotImplementedException();
        }

        public Task Create(DueDetailConfiguration fileContract)
        {
            throw new System.NotImplementedException();
        }
    }
}