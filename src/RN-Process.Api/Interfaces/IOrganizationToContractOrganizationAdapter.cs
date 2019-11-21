using RN_Process.Api.DataAccess.Entities;
using RN_Process.Api.Models;

namespace RN_Process.Api.Interfaces
{
    public interface IOrganizationToContractOrganizationAdapter
    {
        /// <summary>
        ///     Adapt (converter) data from organization to ContractOrganizationModel
        /// </summary>
        /// <param name="fromValue"></param>
        /// <param name="toValue"></param>
        void Adapt(Organization fromValue, ContractOrganization toValue);


        /// <summary>
        /// </summary>
        /// <param name="fromValue"></param>
        /// <param name="organization"></param>
        void Adapt(ContractOrganization fromValue, Organization organization);
    }
}