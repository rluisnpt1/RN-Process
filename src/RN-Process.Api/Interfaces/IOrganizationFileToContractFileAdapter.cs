using System.Collections.Generic;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Api.Models;

namespace RN_Process.Api.Interfaces
{
    public interface IOrganizationFileToContractFileAdapter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromValue"></param>
        /// <param name="toValue"></param>
        void AdaptFile(OrganizationFile fromValue, FileDataContract toValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromValue"></param>
        /// <param name="toValue"></param>
        void AdaptFile(IEnumerable<OrganizationFile> fromValue, IEnumerable<FileDataContract> toValue);

    }
}