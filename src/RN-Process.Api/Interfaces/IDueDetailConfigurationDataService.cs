using System.Collections.Generic;
using System.Threading.Tasks;
using RN_Process.Api.Models;

namespace RN_Process.Api.Interfaces
{
    public interface IDueDetailDataService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DueDetail> GetDueDetailById(string id);

        /// <summary>
        /// Get Configuration list of configuration of organization By DueDetail Id
        /// </summary>
        /// <param name="dueDetailId"> DueDetailId</param>
        /// <returns></returns>
        Task<IList<DueDetailConfiguration>> GetDueDetailConfigurationByDueDetailId(string dueDetailId);
    }
}