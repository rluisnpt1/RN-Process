﻿using System;
using System.Collections.Generic;
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


            foreach (var item in fromValue.TermDetails.GetTermDetails(fromValue.Terms.GetTermOrg(fromValue.Id, fromValue.OrgCode)))
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

                var config = new DueDetailConfiguration();
                config.Id = configTemp.Id;
                config.CommunicationType = configTemp.CommunicationType;
                config.LinkToAccess = configTemp.LinkToAccess;
                config.LinkToAccessType = configTemp.LinkToAccessType;
                config.TypeOfResponse = configTemp.TypeOfResponse;
                config.RequiredLogin = configTemp.RequiredLogin;
                config.AuthenticationLogin = configTemp.AuthenticationLogin;
                config.AuthenticationPassword = configTemp.AuthenticationPassword.Length > 0 ? Encoding.ASCII.GetString(configTemp.AuthenticationPassword) : "";
                config.HostkeyFingerPrint = configTemp.HostKeyFingerPrint.Length > 0 ? Encoding.ASCII.GetString(configTemp.HostKeyFingerPrint) : "";
                config.AuthenticationCodeApp = configTemp.AuthenticationCodeApp;
                config.PathToOriginFile = configTemp.PathToOriginFile;
                config.PathToDestinationFile = configTemp.PathToDestinationFile;
                config.PathToFileBackupAtClient = configTemp.PathToFileBackupAtClient;
                config.FileDelimiter = configTemp.FileDelimiter;
                toValue.DueDetailConfigs.Add(config);
            }
        }

        /// <summary>
        /// 
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

    }
}