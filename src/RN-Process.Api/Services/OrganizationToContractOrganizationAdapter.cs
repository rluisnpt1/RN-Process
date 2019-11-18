using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Api.Models;
using RN_Process.Shared.Commun;

namespace RN_Process.Api.Services
{
    public class OrganizationToContractOrganizationAdapter
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

            toValue.Id = fromValue.Id;
            toValue.CodOrg = fromValue.OrgCode;
            toValue.Description = fromValue.Description;
            toValue.ContractNumber = fromValue.Terms.GetTermOrg(fromValue.Id, fromValue.OrgCode).TermNumber;
            toValue.CreatedDate = fromValue.CreatedDate;
            toValue.CreatedBy = fromValue.CreatedBy;
            toValue.UpdateBy = fromValue.ModifiedBy;
            toValue.ChangedDate = fromValue.ModifiedDate;
            toValue.IsDeleted = fromValue.Active;


            foreach (var item in fromValue.TermDetails.GetTermDetails(
                fromValue.Terms.GetTermOrg(fromValue.Id, fromValue.OrgCode)))
            {
                toValue.DueId = item.TermId;

                var tempDt = new DueDetail
                {
                    Id = item.Id,
                    DebtCode = item.DebtCode,
                    TermsType = item.TermsType
                };

                toValue.DueDetails.Add(tempDt);


                var configTemp = item.TermDetailConfigs.GetTermDetailConfiguration(item.Id, item.OrgCode);

                var config = new DueDetailConfiguration
                {
                    Id = configTemp.Id,
                    CommunicationType = configTemp.CommunicationType,
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

                toValue.DueDetails.Select(x => x.DueDetailConfigs.Select(s => config));
                // toValue.DueDetailConfigs.Add(config);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="fromValues"></param>
        /// <param name="toValues"></param>
        public void Adapt(IEnumerable<Organization> fromValues, IList<ContractOrganization> toValues)
        {
            if (fromValues == null)
                throw new ArgumentNullException("fromValues", "fromValues is null.");

            ContractOrganization toValue;

            foreach (var fromValue in fromValues)
            {
                toValue = new ContractOrganization();

                Adapt(fromValue, toValue);

                toValues.Add(toValue);
            }
        }

        public void Adapt(ContractOrganization fromValue, Organization organization)
        {
            Guard.Against.Null(fromValue, nameof(ContractOrganization));
            Guard.Against.Null(organization, nameof(Organization));

            AdaptDueDetail(fromValue, organization);
        }

        private void AdaptDueDetail(ContractOrganization fromValue, Organization organization)
        {
            foreach (var itemDetailModel in fromValue.DueDetails)
                if (fromValue.IsDeleted == false)
                    foreach (var dueDetailConfiguration in itemDetailModel.DueDetailConfigs)
                        organization.AddTerm(itemDetailModel.Id,
                            fromValue.ContractNumber,
                            itemDetailModel.DebtCode,
                            itemDetailModel.TermsType,
                            dueDetailConfiguration.CommunicationType,
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
                            dueDetailConfiguration.FileHeaderColumns,
                            dueDetailConfiguration.AvailableFieldsColumns
                        );
                else if (fromValue.IsDeleted && !string.IsNullOrWhiteSpace(itemDetailModel.Id))
                    organization.RemoveTerms(itemDetailModel.Id);
        }
    }
}