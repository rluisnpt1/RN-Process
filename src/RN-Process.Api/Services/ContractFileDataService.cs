using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Api.Interfaces;
using RN_Process.Api.Models;
using RN_Process.DataAccess.MongoDb;
using RN_Process.Shared.Commun;
using RN_Process.Shared.Enums;

namespace RN_Process.Api.Services
{
    public class ContractFileDataService : IContractFileDataService
    {
        private readonly OrganizationFileToContractFileAdapter _adapter;
        private readonly IRepositoryMongo<TermDetailConfig> _configRepositoryInstance;
        private readonly IRepositoryMongo<OrganizationFile> _repositoryInstance;

        public ContractFileDataService(
            IRepositoryMongo<OrganizationFile> repositoryInstance,
            IRepositoryMongo<TermDetailConfig> configRepositoryInstance)
        {
            _repositoryInstance = repositoryInstance;
            _configRepositoryInstance = configRepositoryInstance;
            _adapter = new OrganizationFileToContractFileAdapter();
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FileDataContract> GetOrganizationFileById(string id)
        {
            var match = await _repositoryInstance.GetByIdAsync(id);

            if (match == null) return null;

            var toOrganizationFile = new FileDataContract();
            _adapter.AdaptFile(match, toOrganizationFile);

            return toOrganizationFile;
        }

        /// <summary>
        ///     get all files by codcredor
        /// </summary>
        /// <param name="codorg"></param>
        /// <returns></returns>
        public async Task<IList<FileDataContract>> GetOrganizationFileByOrgCod(string codorg)
        {
            Guard.Against.NullOrWhiteSpace(codorg, nameof(codorg));

            var fromDbManyFiles = await _repositoryInstance.GetEqualField("OrgCode", codorg);

            if (fromDbManyFiles == null) return null;

            var dbManyFiles = fromDbManyFiles.ToList();

            var toValues = new List<FileDataContract>();
            if (dbManyFiles.Count > 0)
            {
                dbManyFiles.Select(x => x.OrgCode.Trim().ToUpper().Equals(codorg.Trim().ToUpper()));
                _adapter.AdaptFile(dbManyFiles, toValues);
            }

            return toValues;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileContract"></param>
        /// <returns></returns>
        public async Task CreateOrganizationFile(FileDataContract fileContract)
        {
            Guard.Against.Null(fileContract, nameof(fileContract));

            //retrieve OrganizationConfigurations based on orgCode 
            //TODO change to verify type of debt
            var config = await _configRepositoryInstance.GetEqualField("OrgCode", fileContract.OrgCode);
            var configResult = config.FirstOrDefault();
            if (config == null)
                throw new Exception("configuration of organization was not found.");

            //Verify if exists document with the same name
            var allFilesDb = _repositoryInstance.GetEqualField("OrgCode", fileContract.OrgCode);
            var match = allFilesDb.Result.FirstOrDefault(x => x.FileDescription.Trim().Equals(fileContract.FileDescription));


            if (match == null)
            {
                Enum.TryParse<StatusType>(fileContract.Status, true, out var status);
                Boolean.TryParse(fileContract.FileMigrated, out var fileMigrated);

                var org = new OrganizationFile(string.Empty, fileContract.FileDescription,
                    fileContract.FileSize, fileContract.FileFormat, fileContract.FileLocationOrigin,
                    fileContract.LocationToCopy, status, fileMigrated,
                    null, configResult, fileContract.AllDataInFile);

                _repositoryInstance.Add(org);

                fileContract.Id = org.Id;
            }
            else
            {
                throw new InvalidOperationException(
                    $"Can not save File '{match.FileDescription + " and " + match.OrgCode}' already exist.");
            }
        }
    }
}