using System;
using System.Collections.Generic;
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

        public IList<ContractOrganization> GetContractOrganization()
        {
            var allPeople = GetAll();

            var organizationWhoWereContract =
                allPeople.Result.Where(temp => temp.Terms != null && temp.TermDetails != null);

            return ToContractOrganizations(organizationWhoWereContract);
        }

        public IList<ContractOrganization> Search(string codOrg, string debtId, int contractNumber)
        {
            var allContracts = GetContractOrganization();
            IEnumerable<ContractOrganization> returnValues =
                allContracts;

            if (String.IsNullOrWhiteSpace(codOrg) == false)
            {
                returnValues =
                    returnValues.Where(p => p.CodOrg.ToLower().Contains(codOrg.ToLower()));
            }

            return allContracts;
        }

        private IList<ContractOrganization> ToContractOrganizations(IEnumerable<Organization> organizationWhoWereContract)
        {
            var returnValues = new List<ContractOrganization>();

            _adapter.Adapt(organizationWhoWereContract, returnValues);


            return returnValues;
        }

        
    }
}