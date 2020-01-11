using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Api.Interfaces;
using RN_Process.Api.Models;
using RN_Process.DataAccess.MongoDb;
using RN_Process.Shared.Commun;
using static System.String;

namespace RN_Process.Api.Services
{
    /// <summary>
    ///     here
    /// </summary>
    public class ContractOrganizationServices : IContractOrganizationDataServices
    {
        private readonly IRepositoryMongo<Organization> _repositoryInstance;
        private IValidatorStrategy<ContractOrganization> _validatorInstance;
        private readonly OrganizationToContractOrganizationAdapter _adapter;

        public ContractOrganizationServices(IRepositoryMongo<Organization> 
            repository, IValidatorStrategy<ContractOrganization> validatorInstance)
        {
            _repositoryInstance = repository;
            _validatorInstance = validatorInstance;
            _adapter = new OrganizationToContractOrganizationAdapter();
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
            var match = _repositoryInstance.GetByIdAsync(organizationId);

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
            var allData = _repositoryInstance.GetAllAsync();

            var match = allData.Result.FirstOrDefault(x => x.OrgCode == organizationFromModel.CodOrg);

            if (match == null)
            {
                var org = new Organization(string.Empty, organizationFromModel.Description, organizationFromModel.CodOrg);
                _adapter.Adapt(organizationFromModel, org);

                _repositoryInstance.Add(org);

                organizationFromModel.Id = org.Id;
            }
            else
            {
                throw new InvalidOperationException(
                    $"Can not save Organization {organizationFromModel.CodOrg} already exist.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  IEnumerable<ContractOrganization> GetContractOrganizations()
        {
            var allPeople = _repositoryInstance.GetAllAsync();

            var organizationWhoWereContract =
                allPeople.Result.Where(temp => temp.Terms != null);// && temp.TermDetails != null);

            return ToContractOrganizations(organizationWhoWereContract);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        /// <param name="codOrg"></param>
        /// <returns></returns>
        public IEnumerable<ContractOrganization> Search(string description, string codOrg)
        {
            var allContracts = GetContractOrganizations();

            IEnumerable<ContractOrganization> returnValues = allContracts;

            if (IsNullOrWhiteSpace(description) == false)
            {
                returnValues =
                    returnValues.Where(p => p.CodOrg.ToLower().Contains(description.ToLower()));
            }

            if (IsNullOrWhiteSpace(codOrg) == false)
            {
                returnValues =
                    returnValues.Where(p => p.CodOrg.ToLower().Contains(codOrg.ToLower()));
            }

            return allContracts;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        /// <param name="codOrg"></param>
        /// <param name="contract"></param>
        /// <returns></returns>
        public IEnumerable<ContractOrganization> Search(string description, string codOrg, int contract)
        {
            var allContracts = GetContractOrganizations();

            IEnumerable<ContractOrganization> returnValues = allContracts;

            if (IsNullOrWhiteSpace(description) == false)
            {
                returnValues =
                    returnValues.Where(p => p.CodOrg.ToLower().Contains(description.ToLower()));
            }

            if (IsNullOrWhiteSpace(codOrg) == false)
            {
                returnValues =
                    returnValues.Where(p => p.CodOrg.ToLower().Contains(codOrg.ToLower()));
            }

            if (contract > 0)
            {
                returnValues =
                    returnValues.Where(p => p.ContractNumber == contract);
            }

            return allContracts;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeOrg"></param>
        /// <returns></returns>
        public IEnumerable<ContractOrganization> Search(string codeOrg)
        {
            var allContracts = GetContractOrganizations();

            IEnumerable<ContractOrganization> returnValues = allContracts;

            if (IsNullOrWhiteSpace(codeOrg) == false)
            {
                returnValues =
                    returnValues.Where(p => p.CodOrg.ToLower().Contains(codeOrg.ToLower()));
            }

            return allContracts;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationWhoWereContract"></param>
        /// <returns></returns>
        private IList<ContractOrganization> ToContractOrganizations(IEnumerable<Organization> organizationWhoWereContract)
        {
            var returnValues = new List<ContractOrganization>();

            _adapter.Adapt(organizationWhoWereContract, returnValues);


            return returnValues;
        }

       
    }
}