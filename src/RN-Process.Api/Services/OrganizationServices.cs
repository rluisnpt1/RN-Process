using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RN_Process.Api.DataAccess;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Api.DataAccess.Repositories;
using RN_Process.Api.Models;
using RN_Process.DataAccess;
using RN_Process.DataAccess.MongoDb;

namespace RN_Process.Api.Services
{
    public class OrganizationServices : BaseMongoRepository<Organization, string>
    {
        private readonly IOrganizationToContractOrganizationAdapter _adapter;
        
        public OrganizationServices(IOptions<MongoDbSettings> settings, 
            IOrganizationToContractOrganizationAdapter adapter) : base(settings)
        {
            _adapter = adapter;
        }

        public ContractOrganization GetContractOrganizationById(string id)
        {
            var match = GetOneAsync(id);

            if (match.Result == null)
            {
                return null;
            }

            var toContractOrganization = new ContractOrganization();
            _adapter.Adapt(match.Result, toContractOrganization);
            return toContractOrganization;
        }
    }
}
