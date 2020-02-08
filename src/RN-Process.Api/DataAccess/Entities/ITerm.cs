using System;
using System.Collections.Generic;
using RN_Process.Shared.Enums;

namespace RN_Process.Api.DataAccess.Entities
{
    public interface ITerm
    {
        int TermNumber { get; }
        string OrgCode { get; }
        string OrganizationId { get; }
        IOrganization Organization { get; set; }

        ICollection<ITermDetail> TermDetails { get; set; }

        /// <summary>
        /// </summary>
        bool Deleted { get; set; }

        bool Active { get; set; }

        /// <summary>
        /// </summary>
        string Id { get; set; }

        void UpdateTermTermById(string id, int debtCode, TermsType term, bool active);
    }
}