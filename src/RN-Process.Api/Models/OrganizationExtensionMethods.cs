using System.Collections.Generic;
using System.Linq;
using RN_Process.Api.DataAccess.Entities;

namespace RN_Process.Api.Models
{
    public static class OrganizationExtensionMethods
    {
        public static ITerm GetTermOrg(this ICollection<ITerm> terms, string orgId, string orgcod)
        {
            var returnValue = (from temp in terms
                    where temp.OrganizationId == orgId
                          && temp.OrgCode == orgcod
                    select temp
                ).FirstOrDefault();

            return returnValue;
        }

        public static ITermDetail GetTermDetail(this ICollection<ITermDetail> details, int debtCode, string orgcod)
        {
            var returnValue = (from temp in details
                    where temp.DebtCode == debtCode
                          && temp.OrgCode == orgcod
                    select temp
                ).FirstOrDefault();

            return returnValue;
        }

        public static ITermDetailConfig GetTermDetailConfiguration(this ICollection<ITermDetailConfig> detailsConfig,
            string termDetailId, string orgcod)
        {
            var returnValue = (from temp in detailsConfig
                    where temp.TermDetailId == termDetailId
                          && temp.OrgCode == orgcod
                    select temp
                ).FirstOrDefault();

            return returnValue;
        }

        public static ICollection<ITerm> GetTerms(this ICollection<ITerm> terms, string orgId, string orgcod)
        {
            var returnValue = (from temp in terms
                    where temp.OrganizationId == orgId
                          && temp.OrgCode == orgcod
                    select temp
                ).ToList();

            return returnValue;
        }

        public static int GetTermNumber(this ICollection<ITerm> terms, string orgId, string orgcod)
        {
            var returnValue = (from temp in terms
                    where temp.OrganizationId == orgId
                          && temp.OrgCode == orgcod
                    select temp
                ).FirstOrDefault();

            return returnValue?.TermNumber ?? 0;
        }

        public static ICollection<ITermDetail> GetTermDetails(this ICollection<ITermDetail> details, Term term)
        {
            var returnValue = (from temp in details
                    where temp.TermId == term.Id
                          && temp.OrgCode == term.OrgCode
                    select temp
                ).ToList();

            return returnValue;
        }
    }
}