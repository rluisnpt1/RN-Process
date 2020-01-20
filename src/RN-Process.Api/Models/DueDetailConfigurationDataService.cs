using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Api.Interfaces;
using RN_Process.Api.Services;
using RN_Process.DataAccess.MongoDb;
using RN_Process.Shared.Commun;

namespace RN_Process.Api.Models
{
    public class DueDetailDataService : IDueDetailDataService
    {
        private readonly IRepositoryMongo<TermDetail> _repositoryInstance;
        private IValidatorStrategy<DueDetail> _validatorInstance;
        private readonly OrganizationToContractOrganizationAdapter _adapter;

        public DueDetailDataService(IRepositoryMongo<TermDetail> repositoryInstance, IValidatorStrategy<DueDetail> validatorInstance)
        {
            _repositoryInstance = repositoryInstance;
            _validatorInstance = validatorInstance;
        }

        public Task<DueDetail> GetDueDetailConfigurationById(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IList<DueDetail>> GetDueDetailConfigurationOrgCod(string codorg)
        {
            throw new System.NotImplementedException();
        }

        public Task Create(DueDetail dueDetail)
        {
            throw new System.NotImplementedException();
        }
    }
}