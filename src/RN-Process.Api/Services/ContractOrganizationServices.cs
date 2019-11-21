using System;
using System.Linq;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Api.Interfaces;
using RN_Process.Api.Models;
using RN_Process.DataAccess.MongoDb;
using RN_Process.Shared.Commun;

namespace RN_Process.Api.Services
{
    /// <summary>
    ///     here
    /// </summary>
    public class ContractOrganizationServices : BaseRepositoryMongo<Organization, string>,
        IContractOrganizationDataServices
    {
        private readonly IOrganizationToContractOrganizationAdapter _adapter;

        public ContractOrganizationServices(IMongoContext context,
            IOrganizationToContractOrganizationAdapter adapter) : base(context)
        {
            _adapter = adapter;
        }

        /// <summary>
        ///     Get contracts by id. Here works as a buffer communication between adapter and service.
        ///     Converter data from dataBase to model
        ///     and delivery to service
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public ContractOrganization GetContractOrganizationById(string organizationId)
        {
            var match = GetById(organizationId);

            if (match.Result == null) return null;

            var toContractOrganization = new ContractOrganization();
            _adapter.Adapt(match.Result, toContractOrganization);
            return toContractOrganization;
        }

        /// <summary>
        /// </summary>
        /// <param name="organizationFromModel"></param>
        public void CreateContractOrganization(ContractOrganization organizationFromModel)
        {
            Guard.Against.Null(organizationFromModel, nameof(organizationFromModel));

            //GET ALL DATA --PLEASE CHANGE IT LATER
            var allData = GetAll();

            var match = allData.Result.FirstOrDefault(x => x.OrgCode == organizationFromModel.CodOrg);

            if (match == null)
            {
                var org = new Organization(organizationFromModel.Description, organizationFromModel.CodOrg);
                _adapter.Adapt(organizationFromModel, org);

                Add(org);

                organizationFromModel.Id = org.Id;
            }
            else
            {
                throw new InvalidOperationException(
                    $"Can not save Organization {organizationFromModel.CodOrg} already exist.");
            }
        }
    }
}