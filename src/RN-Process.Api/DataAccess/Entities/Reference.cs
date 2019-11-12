
using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RN_Process.DataAccess;

namespace RN_Process.Api.DataAccess.Entities
{
    public class Reference : AuditableEntity<string>
    {
        //runtime execution
        protected Reference()
        {
            
        }

        public Reference(string uniqCode, string referencesTypeId, ReferencesType referencesType)
        {

            Id = ObjectId.GenerateNewId().ToString();

            UniqCode = uniqCode;
            ReferencesTypeId = referencesTypeId;
            ReferencesType = referencesType;
        }
        public string UniqCode { get; set; }

        public string ReferencesTypeId { get; set; }
        public virtual ReferencesType ReferencesType  { get; set; }
    }
}
