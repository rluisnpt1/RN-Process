using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Api.Interfaces;
using RN_Process.Api.Models;
using RN_Process.Shared.Commun;

namespace RN_Process.Api.Services
{
    public class OrganizationFileToContractFileAdapter : IOrganizationFileToContractFileAdapter
    {
        public void AdaptFile(OrganizationFile fromValue, FileDataContract toValue)
        {
            Guard.Against.Null(fromValue, nameof(OrganizationFile));
            Guard.Against.Null(toValue, nameof(FileDataContract));

            toValue.Id = fromValue.Id;
            toValue.AllDataInFile = fromValue.AllDataInFile;
            toValue.FileFormat = fromValue.FileFormat;
            toValue.FileLocationOrigin = fromValue.FileLocationOrigin;
            toValue.FileMigrated = fromValue.FileMigrated.ToString();
            toValue.FileSize = fromValue.FileSize;
            toValue.LocationToCopy = fromValue.LocationToCopy;
            toValue.OrgCode = fromValue.OrgCode;
            toValue.Status = fromValue.Status.ToString();
            toValue.FileDescription = fromValue.FileDescription;
            toValue.FileMigratedOn = fromValue.FileMigratedOn;
        }

        public void AdaptFile(IEnumerable<OrganizationFile> fromValue, IEnumerable<FileDataContract> toValue)
        {
            var fileDataContracts = toValue.ToList();
            var organizationFiles = fromValue.ToList();

            Guard.Against.Zero(organizationFiles.Count(), nameof(OrganizationFile));
            Guard.Against.Zero(fileDataContracts.Count(), nameof(FileDataContract));


            foreach (var organizationFile in organizationFiles)
            {
                var value = new FileDataContract
                {
                    Id = organizationFile.Id,
                    AllDataInFile = organizationFile.AllDataInFile,
                    FileFormat = organizationFile.FileFormat,
                    FileLocationOrigin = organizationFile.FileLocationOrigin,
                    FileMigrated = organizationFile.FileMigrated.ToString(),
                    FileSize = organizationFile.FileSize,
                    LocationToCopy = organizationFile.LocationToCopy,
                    OrgCode = organizationFile.OrgCode,
                    Status = organizationFile.Status.ToString(),
                    FileDescription = organizationFile.FileDescription,
                    FileMigratedOn = organizationFile.FileMigratedOn
                };
                var dt = new[] { value };
                toValue = fileDataContracts.Concat(dt);
            }
        }
    }
}
