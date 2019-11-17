using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RN_Process.DataAccess;
using RN_Process.Shared.Commun;
using RN_Process.Shared.Enums;

namespace RN_Process.Api.DataAccess.Entities
{
    public class Term : AuditableEntity<string>
    {
        //[BsonIgnore] private ICollection<TermDetailConfig> _configMapping;
        [BsonIgnore] private ICollection<TermDetail> _termDetail;

        //Runtime execution
        protected Term()
        {
        }

        public Term(int termNumber, Organization organization)
        {
            Id = ObjectId.GenerateNewId().ToString();
            SetTermNumber(termNumber);
            SetCustomer(organization);
            Active = true;
            Deleted = false;
        }

        public virtual Organization Organization { get; set; }


        public int TermNumber { get; private set; }

        public virtual string OrgCode { get; private set; }
        public string OrganizationId { get; private set; }


        public virtual ICollection<TermDetail> TermDetails
        {
            get; private set;
        }
        private void SetCustomer(Organization organization)
        {
            Guard.Against.Null(organization, nameof(organization));
            OrganizationId = organization.Id;
            OrgCode = organization.OrgCode;
            Organization = organization;
            TermDetails = new List<TermDetail>();

        }

        private void SetTermNumber(int termNumber)
        {
            Guard.Against.Zero(termNumber, nameof(TermNumber));
            TermNumber = termNumber;
        }

        public void AddTermDetail(string id, int debtCode, TermsType termType, TermDetailConfig detailConfig, bool active = true)
        {
            Guard.Against.Null(debtCode, nameof(debtCode));
            Guard.Against.Zero(debtCode, nameof(debtCode));

            if (!string.IsNullOrWhiteSpace(id))
                UpdateTermTermById(id, debtCode, termType, detailConfig, active);
            else
                AddNewTermDetails(debtCode, termType, detailConfig);
        }

        private void AddNewTermDetails(int debtCode, TermsType termType, TermDetailConfig detailConfig)
        {
            var fact = new TermDetail(debtCode, termType, this);

            TermDetails.Add(fact);
            fact.AddDetailConfig(detailConfig);
        }


        public void UpdateTermTermById(string id, int debtCode, TermsType term, TermDetailConfig detailConfig, bool active)
        {
            TermDetail termdet = null;
            var foundIt = false;

            if (!string.IsNullOrEmpty(id))
                termdet = TermDetails.FirstOrDefault(temp => temp.Id.Equals(id)
                                                             && temp.Term.Id == Id
                                                             && temp.Term.OrgCode == OrgCode);

            if (termdet == null)
            {
                termdet = new TermDetail(debtCode, term, this);
            }
            else
            {
                foundIt = true;
                termdet.ModifiedDate = DateTime.UtcNow;
                termdet.ModifiedBy = "System-- need change for user";
                termdet.Active = active;
                termdet.Deleted = !active;

                var config = termdet.TermDetailConfigs.FirstOrDefault(temp => temp.TermDetailId == termdet.Id);
                if (config != null)
                {
                    config.Active = termdet.Active;
                    config.Deleted = termdet.Deleted;

                    termdet.UpdateTermConfiguration(config, config.Active);
                }
            }

            if (foundIt == false) TermDetails.Add(termdet);
        }

    }
}