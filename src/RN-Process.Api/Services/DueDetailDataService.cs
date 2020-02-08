using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Api.Interfaces;
using RN_Process.Api.Models;
using RN_Process.DataAccess.MongoDb;
using RN_Process.Shared.Commun;

namespace RN_Process.Api.Services
{
    public class DueDetailDataService : IDueDetailDataService
    {
        private readonly IRepositoryMongo<TermDetail> _repositoryInstance;
        private IValidatorStrategy<DueDetail> _validatorInstance;
        private readonly OrganizationToContractOrganizationAdapter _adapter;

        public DueDetailDataService(IRepositoryMongo<TermDetail> repositoryInstance, IValidatorStrategy<DueDetail>
            validatorInstance)
        {
            _repositoryInstance = repositoryInstance;
            _validatorInstance = validatorInstance;
            _adapter = new OrganizationToContractOrganizationAdapter();
        }

        /// <summary>
        /// Get Due detail 
        /// </summary>
        /// <param name="duedetailId"></param>
        /// <returns></returns>
        public async Task<DueDetail> GetDueDetailById(string duedetailId)
        {
            Guard.Against.NullOrWhiteSpace(duedetailId, nameof(duedetailId));
            var match = await _repositoryInstance.GetByIdAsync(duedetailId);

            if (match == null) return null;

            var toDueDetail = new DueDetail();
            _adapter.AdaptTermDetailConfig(match, toDueDetail);


            return toDueDetail;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dueDetailId"></param>
        /// <returns></returns>
        public async Task<IList<DueDetailConfiguration>> GetDueDetailConfigurationByDueDetailId(string dueDetailId)
        {
            Guard.Against.NullOrWhiteSpace(dueDetailId, nameof(dueDetailId));
            var match = await _repositoryInstance.GetByIdAsync(dueDetailId);

            if (match == null) return null;

            var configuration = new List<DueDetailConfiguration>();
            _adapter.AdaptTermDetailConfig((IEnumerable<TermDetailConfig>) match.TermDetailConfigs, configuration.AsEnumerable());
            return configuration;
        }

    }
}