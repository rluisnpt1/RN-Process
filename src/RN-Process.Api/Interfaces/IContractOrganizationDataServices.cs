using System.Collections.Generic;
using RN_Process.Api.Models;

namespace RN_Process.Api.Interfaces
{
    public interface IContractOrganizationDataServices
    {
        /// <summary>
        ///     Get contracts by id. Here works as a buffer communication between adapter and service.
        ///     Converter data from dataBase to model
        ///     and delivery to service
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        ContractOrganization GetContractOrganizationById(string organizationId);

        /// <summary>
        /// </summary>
        /// <param name="organizationFromModel"></param>
        void CreateContractOrganization(ContractOrganization organizationFromModel);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<ContractOrganization> GetContractOrganizations();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeOrg"></param>
        /// <returns></returns>
        IEnumerable<ContractOrganization> Search(string codeOrg);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        /// <param name="codeOrg"></param>
        /// <returns></returns>
        IEnumerable<ContractOrganization> Search(string description, string codeOrg);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        /// <param name="codeOrg"></param>
        /// <param name="contractNumber"></param>
        /// <returns></returns>
        IEnumerable<ContractOrganization> Search(string description, string codeOrg, int contractNumber);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        bool OrganizationSyncRepositories(string organizationId);
    }
}