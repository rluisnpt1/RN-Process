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
            toValue.ContractNumber = fromValue.Terms.GetTermNumber(fromValue.Id, fromValue.OrgCode);
            foreach (var item in fromValue.TermDetails.GetTermDetails(fromValue.Terms.GetTerm(fromValue.Id,fromValue.OrgCode)))
            {
                //var configTemp = item.TermDetailConfigs.GetTermDetailConfiguration(item.Id, item.OrgCode);

                ////var temp = new Due(item.DebtCode, item.TermsType, configTemp.CommunicationType, configTemp.LinkToAccess,
                ////    configTemp.LinkToAccessType, configTemp.TypeOfResponse, configTemp.RequiredLogin,
                ////    configTemp.AuthenticationLogin, Encoding.ASCII.GetString(configTemp.AuthenticationPassword),
                ////    Encoding.ASCII.GetString(configTemp.HostKeyFingerPrint),
                ////    configTemp.AuthenticationCodeApp, configTemp.PathToOriginFile, configTemp.PathToDestinationFile,
                ////    configTemp.PathToFileBackupAtClient, configTemp.FileDelimiter, configTemp.FileHeaderColumns,
                ////    configTemp.AvailableFieldsColumns, configTemp.Active) {Id = configTemp.Id};

                //toValue.Dues.Add(temp);
            }
        }
    }
}