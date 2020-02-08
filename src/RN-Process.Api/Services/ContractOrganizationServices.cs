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
        public async Task CreateContractOrganization(ContractOrganization organizationFromModel)
        {
            Guard.Against.Null(organizationFromModel, nameof(organizationFromModel));
            Guard.Against.NullOrWhiteSpace(organizationFromModel.CodOrg, nameof(organizationFromModel.CodOrg));
            Guard.Against.Zero(organizationFromModel.ContractNumber, nameof(organizationFromModel.ContractNumber));

            //GET ALL DATA --PLEASE CHANGE IT LATER
            var allData = await _repositoryInstance.GetAllAsync();

            var match = allData.FirstOrDefault(x => x.OrgCode == organizationFromModel.CodOrg);

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
        public IEnumerable<ContractOrganization> GetContractOrganizations()
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

            var contractOrganizations = allContracts.ToList();
            IEnumerable<ContractOrganization> returnValues = contractOrganizations;

            if (IsNullOrWhiteSpace(description) == false)
            {
                returnValues =
                    returnValues.Where(p => p.Description.ToLower().Contains(description.ToLower()));
            }

            if (IsNullOrWhiteSpace(codOrg) == false)
            {
                returnValues =
                    returnValues.Where(p => p.CodOrg.ToLower().Contains(codOrg.ToLower()));
            }

            return contractOrganizations;
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
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public async Task<bool> OrganizationSyncRepositories(string organizationId)
        {
            Guard.Against.NullOrWhiteSpace(organizationId, nameof(organizationId));
            var match = GetContractOrganizationById(organizationId);

            var isValid = false;

            if (match != null)
            {
                var org = new Organization(match.Id, match.Description, match.CodOrg);
                _adapter.Adapt(match, org);

                try
                {
                    if (org.Terms != null)
                        foreach (var item in org.Terms.Select(x =>
                            x.TermDetails.GetTermDetails((Term) org.Terms.GetTermOrg(org.Id, org.OrgCode))))
                            foreach (var termDetail in item)
                            {
                                var configTemp =
                                    termDetail.TermDetailConfigs.GetTermDetailConfiguration(termDetail.Id,
                                        termDetail.OrgCode);

                                configTemp.FtpDownloadingTheMostRecentFileRemoteDir();
                            }

                    isValid = true;
                }
                catch (InvalidOperationException ex)
                {
                    throw new InvalidOperationException($"Could not sync the repository ORG: {match.CodOrg} - ERROR: {ex.Message}");
                }
            }

            return isValid;
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

            return returnValues;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationOrgCode"></param>
        /// <returns></returns>
        //public bool OrganizationSyncRepositories(string organizationId)
        //{
        //    ///Guard.Against.Null(organizationId, nameof(organizationId));
        //    //var match = GetContractOrganizationById(organizationId);
        //    //if (match == null)
        //    //    return false;
        //    return true;
        //}
    }
}