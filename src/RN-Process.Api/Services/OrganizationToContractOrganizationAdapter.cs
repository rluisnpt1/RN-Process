using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Api.Interfaces;
using RN_Process.Api.Models;
using RN_Process.Shared.Commun;
using RN_Process.Shared.Enums;

namespace RN_Process.Api.Services
{
    public class OrganizationToContractOrganizationAdapter : IOrganizationToContractOrganizationAdapter
    {
        /// <summary>
        ///     Adapt (converter) data from organization to ContractOrganizationModel
        /// </summary>
        /// <param name="fromValue"></param>
        /// <param name="toValue"></param>
        public void Adapt(Organization fromValue, ContractOrganization toValue)
        {
            Guard.Against.Null(fromValue, nameof(Organization));
            Guard.Against.Null(toValue, nameof(ContractOrganization));
            
            var term = fromValue.Terms.GetTermOrg(fromValue.Id, fromValue.OrgCode);

            toValue.Id = fromValue.Id;
            toValue.CodOrg = fromValue.OrgCode;
            toValue.Description = fromValue.Description;
            toValue.ContractNumber = term?.TermNumber > 0 ? term.TermNumber : 0;
            //toValue.CreatedDate = fromValue.CreatedDate;
            //toValue.CreatedBy = fromValue.CreatedBy;
            //toValue.UpdateBy = fromValue.ModifiedBy;
            //toValue.ChangedDate = fromValue.UpdatedDate;
            toValue.IsDeleted = fromValue.Deleted;


            if (term != null)
                foreach (var item in term.TermDetails.GetTermDetails(fromValue.Terms.GetTermOrg(fromValue.Id, fromValue.OrgCode)))
                    AdaptTermsDetail(item, toValue);
        }




        /// <summary>
        ///     Convert data from (ContractOrganization  model) to entity Organization
        /// </summary>
        /// <param name="fromValue"></param>
        /// <param name="organization"></param>
        public void Adapt(ContractOrganization fromValue, Organization organization)
        {
            Guard.Against.Null(fromValue, nameof(ContractOrganization));
            Guard.Against.Null(organization, nameof(Organization));

            AdaptDueDetail(fromValue, organization);
        }

        public void Adapt(IEnumerable<Organization> fromOrganization, List<ContractOrganization> toContractOrg)
        {
            if (fromOrganization == null)
                throw new ArgumentNullException("fromValues", "fromValues is null.");

            ContractOrganization toValue;

            foreach (var fromValue in fromOrganization)
            {
                toValue = new ContractOrganization();

                Adapt(fromValue, toValue);

                toContractOrg.Add(toValue);
            }
        }

        /// <summary>
        ///     Adapt detail from entity to model
        /// </summary>
        /// <param name="item"></param>
        /// <param name="toValue"></param>
        public static void AdaptTermsDetail(TermDetail item, ContractOrganization toValue)
        {
            Guard.Against.Null(toValue, nameof(ContractOrganization));
            Guard.Against.Null(item, nameof(TermDetail));
            toValue.DueId = item.TermId;

            var tempDt = new DueDetail
            {
                Id = item.Id,
                DebtCode = item.DebtCode,
                TermsType = item.TermsType.ToString(),
                OrgCode = item.OrgCode,
            };

            var configTemp = item.TermDetailConfigs.GetTermDetailConfiguration(item.Id, item.OrgCode);

            var config = new DueDetailConfiguration
            {
                Id = configTemp.Id,
                OrgCode =item.OrgCode,
                CommunicationType = configTemp.CommunicationType.ToString(),
                LinkToAccess = configTemp.LinkToAccess,
                LinkToAccessType = configTemp.LinkToAccessType,
                TypeOfResponse = configTemp.TypeOfResponse,
                RequiredLogin = configTemp.RequiredLogin,
                AuthenticationLogin = configTemp.AuthenticationLogin,
                AuthenticationPassword = configTemp.AuthenticationPassword.Length > 0
                    ? Encoding.ASCII.GetString(configTemp.AuthenticationPassword)
                    : "",
                HostkeyFingerPrint = configTemp.HostKeyFingerPrint.Length > 0
                    ? Encoding.ASCII.GetString(configTemp.HostKeyFingerPrint)
                    : "",
                AuthenticationCodeApp = configTemp.AuthenticationCodeApp,
                PathToOriginFile = configTemp.PathToOriginFile,
                PathToDestinationFile = configTemp.PathToDestinationFile,
                PathToFileBackupAtClient = configTemp.PathToFileBackupAtClient,
                FileDelimiter = configTemp.FileDelimiter
            };

            toValue.DueDetails.Add(tempDt);
            tempDt.DueDetailConfigs.Add(config);
            //toValue.DueDetails.Select(x => x.DueDetailConfigs.Select(s => config));
        }


        /// <summary>
        /// </summary>
        /// <param name="fromValue"></param>
        /// <param name="organization"></param>
        public void AdaptDueDetail(ContractOrganization fromValue, Organization organization)
        {
            Guard.Against.Null(fromValue, nameof(ContractOrganization));
            Guard.Against.Null(organization, nameof(Organization));

            foreach (var itemDetailModel in fromValue.DueDetails)
            {
                Enum.TryParse<TermsType>(itemDetailModel.TermsType, true, out var typeTerm);


                if (fromValue.IsDeleted.Equals(false))
                    foreach (var dueDetailConfiguration in itemDetailModel.DueDetailConfigs)
                    {
                        Enum.TryParse<FileAccessType>(dueDetailConfiguration.CommunicationType, true, out var fileAccessType);
                        //Boolean.TryParse(dueDetailConfiguration.HasHeader, out var hasHeaderValue);
                        //Boolean.TryParse(dueDetailConfiguration.RequiredLogin, out var hasRequiredLogin);
                      
                        organization.AddTerm(null,
                            fromValue.ContractNumber,
                            itemDetailModel.DebtCode,
                            typeTerm,
                            fileAccessType,
                            string.Empty,
                            dueDetailConfiguration.LinkToAccess,
                            dueDetailConfiguration.LinkToAccessType,
                            dueDetailConfiguration.TypeOfResponse,
                            dueDetailConfiguration.RequiredLogin,
                            dueDetailConfiguration.AuthenticationLogin,
                            dueDetailConfiguration.AuthenticationPassword,
                            dueDetailConfiguration.HostkeyFingerPrint,
                            dueDetailConfiguration.AuthenticationCodeApp,
                            dueDetailConfiguration.PathToOriginFile,
                            dueDetailConfiguration.PathToDestinationFile,
                            dueDetailConfiguration.PathToFileBackupAtClient,
                            string.Empty,
                            dueDetailConfiguration.FileDelimiter,
                            dueDetailConfiguration.HasHeader,
                            dueDetailConfiguration.FileProtectedPassword,
                            dueDetailConfiguration.FileHeaderColumns,
                            dueDetailConfiguration.AvailableFieldsColumns
                        );
                    }
                else if (fromValue.IsDeleted.Equals(true) && !string.IsNullOrWhiteSpace(itemDetailModel.Id))
                    organization.RemoveTerms(itemDetailModel.Id);
            }
        }



        public void AdaptToOrganizationFile(FileDataContract fromFileData, TermDetailConfig configuration)
        {
            Guard.Against.Null(fromFileData, nameof(FileDataContract));
            Guard.Against.Null(configuration, nameof(TermDetailConfig));

            Enum.TryParse<StatusType>(fromFileData.Status, true, out var statusType);
            Boolean.TryParse(fromFileData.FileMigrated, out var fileMigrated);

            configuration.AddOrganizationFile("",
                fromFileData.OrgCode,
                fromFileData.FileDescription,
                fromFileData.FileSize,
                fromFileData.FileFormat,
                fromFileData.FileLocationOrigin,
                fromFileData.LocationToCopy,
                statusType,
                fileMigrated,
                fromFileData.FileMigratedOn,
                fromFileData.AllDataInFile, true);

            //var configTemp = item.TermDetailConfigs.GetTermDetailConfiguration(item.Id, item.OrgCode);

        }

    }
}