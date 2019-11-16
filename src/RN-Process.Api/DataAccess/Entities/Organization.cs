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
    [BsonKnownTypes(typeof(Organization))]
    public class Organization : AuditableEntity<string>
    {
        private static readonly DateTime DefaultDateTime = DateTime.UtcNow;

        [BsonIgnore] private ICollection<Term> _term;
        [BsonIgnore] private ICollection<TermDetail> _termDetail;

        public Organization(string description, string orgCode)
        {
            Id = ObjectId.GenerateNewId().ToString();
            SetDescription(description);
            SetOrgCode(orgCode);
            SetVersion();

            Active = true;
            Deleted = false;
            RowVersion = new byte[0];
        }

        /// <summary>
        /// </summary>
        protected Organization()
        {
        }

        public virtual string OrgCode { get; private set; }
        public string Description { get; private set; }

        public virtual ICollection<Term> Terms
        {
            get { return _term ??= new List<Term>(); }
            protected set => _term = value;
        }


        public virtual ICollection<TermDetail> TermDetails
        {
            get { return _termDetail ??= new List<TermDetail>(); }
            protected set => _termDetail = value;
        }

        /// <summary>
        /// </summary>
        /// <param name="orgCode"></param>
        private void SetOrgCode(string orgCode)
        {
            Guard.Against.NullOrEmpty(orgCode, nameof(orgCode));
            Guard.Against.NullOrWhiteSpace(orgCode, nameof(orgCode));
            Guard.Against.OutOfRange(orgCode.Length, nameof(orgCode), 3, 10);

            OrgCode = orgCode.ToUpper();
        }

        /// <summary>
        /// </summary>
        /// <param name="description"></param>
        private void SetDescription(string description)
        {
            Guard.Against.NullOrEmpty(description, nameof(description));
            Guard.Against.OutOfRange(description.Length, nameof(description), 5, 250);
            Description = description;
        }

        /// <summary>
        ///     Return a version byte
        /// </summary>
        private void SetVersion()
        {
            RowVersion = new byte[0];
        }


        /// <summary>
        /// </summary>
        /// <param name="termNumber"></param>
        /// <param name="typeDebt"></param>
        /// <param name="debtDescription"></param>
        public void AddNewTerm(int termNumber, int typeDebt)
        {
            //create term
            var fact = new Term(termNumber, this);

            //add term to list term list of organization
            Terms.Add(fact);

            //base configuration
            fact.AddTermDetail(null, typeDebt, TermsType.Loan, true, false);

            //add term configuration details to list term configuration details list of organization
            TermDetails = fact.TermDetails;
        }


        /// <summary>
        ///     Add term
        /// </summary>
        /// <param name="id"></param>
        /// <param name="termNumber"></param>
        /// <param name="typeDebt"></param>
        /// <param name="debtDescription"></param>
        public void AddTerm(string id, int termNumber, int typeDebt)
        {
            Guard.Against.Null(termNumber, nameof(termNumber));
            Guard.Against.Zero(termNumber, nameof(termNumber));
            Guard.Against.Null(typeDebt, nameof(typeDebt));
            Guard.Against.Zero(typeDebt, nameof(typeDebt));

            if (!string.IsNullOrEmpty(id))
                UpdateExistingTermById(id, typeDebt, termNumber);
            else
                AddNewTerm(termNumber, typeDebt);
        }

       
        private void UpdateExistingTermById(string id, int debtCode, int termNumber, bool active = true, bool deleted = false)
        {
            Term term = null;
            var foundIt = false;

            if (!string.IsNullOrEmpty(id)) term = Terms.FirstOrDefault(temp => temp.Id == id);
            //for 
            if (term == null)
            {
                term = new Term(termNumber, this);
                term.AddTermDetail(null,debtCode, TermsType.Loan);
            }
            else
            {
                //add update term ?
                foundIt = true;
                term.ModifiedDate = DateTime.UtcNow;
                term.ModifiedBy = "System-- need change for user";
                term.Active = active;
                term.Deleted = deleted;

                var config = term.TermDetails.Where(temp => temp.TermId == term.Id);
                foreach (var item in config)
                    term.UpdateTermTermById(item.Id, item.DebtCode, item.TermsType, active, deleted);
            }

            if (foundIt == false) Terms.Add(term);
        }

        public void RemoveTerms(string id)//, bool softDelete)
        {
            if (string.IsNullOrEmpty(id)) return;

            var matchTerm = Terms.FirstOrDefault(fact => fact.Id == id);

            if (matchTerm == null) return;

            var match = matchTerm.TermDetails.FirstOrDefault(x => x.Term.Id == matchTerm.Id);

            //if (!softDelete)
            //    Terms.Remove(matchTerm);
            //else
            
            if (match == null) return;

            UpdateExistingTermById(matchTerm.Id, match.DebtCode, matchTerm.TermNumber, false, true);
        }
    }
}