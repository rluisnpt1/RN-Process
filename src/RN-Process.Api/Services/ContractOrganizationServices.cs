using System;
using System.Linq;
using Microsoft.Extensions.Options;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Api.Models;
using RN_Process.DataAccess;
using RN_Process.DataAccess.MongoDb;
using RN_Process.Shared.Commun;

namespace RN_Process.Api.Services
{
    /// <summary>
    /// here
    /// </summary>
    public class ContractOrganizationServices
    {
        private readonly IOrganizationToContractOrganizationAdapter _adapter;
        private readonly IRepositoryNoSql<Organization, string> _repository;
        private IOptions<MongoDbSettings> Settings { get; }

        public ContractOrganizationServices(IOptions<MongoDbSettings> settings,
            IOrganizationToContractOrganizationAdapter adapter,
            IRepositoryNoSql<Organization, string> repository)
        {
            Settings = settings;
            _adapter = adapter;
            _repository = repository;
        }

        /// <summary>
        /// Get contracts by id. Here works as a buffer communication between adapter and service.
        /// Converter data from dataBase to model
        /// and delivery to service
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ContractOrganization GetContractOrganizationById(string id)
        {
            var match = _repository.GetOneAsync(id);

            if (match.Result == null)
            {
                return null;
            }

            var toContractOrganization = new ContractOrganization();
            _adapter.Adapt(match.Result, toContractOrganization);
            return toContractOrganization;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationFromModel"></param>
        public void SaveContract(ContractOrganization organizationFromModel)
        {
            Guard.Against.Null(organizationFromModel, nameof(organizationFromModel));

            //GET ALL DATA --PLEASE CHANGE IT LATER
            var allData = _repository.GetAllAsync();

            var match = allData.Result.FirstOrDefault(x => x.OrgCode == organizationFromModel.CodOrg);

            if (match == null)
            {
                var org = new Organization(organizationFromModel.Description, organizationFromModel.CodOrg);
                _adapter.Adapt(organizationFromModel, org);

                _repository.SaveOneAsync(org);
                organizationFromModel.Id = org.Id;
            }
            else
            {
                throw new InvalidOperationException($"Can not save Organization {organizationFromModel.CodOrg} already exist.");
            }

        }
    }
}
