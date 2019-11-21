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
    }
}