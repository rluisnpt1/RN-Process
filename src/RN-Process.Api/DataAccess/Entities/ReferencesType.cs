using System.Collections.Generic;
using MongoDB.Bson;
using RN_Process.DataAccess;

namespace RN_Process.Api.DataAccess.Entities
{
    public class ReferencesType : Entity<string>
    {
        //runtime execution
        protected ReferencesType()
        {
        }

        public ReferencesType(string uniqCode)
        {
            Id = ObjectId.GenerateNewId().ToString();
            References = new List<Reference>();
            UniqCode = uniqCode;
        }

        public virtual ICollection<Reference> References { get; set; }
        public string UniqCode { get; set; }
    }
}