﻿
using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RN_Process.DataAccess;

namespace RN_Process.Api.DataAccess.Entities
{
    public class Reference : Entity<string>
    {
        //runtime execution
        protected Reference()
        {
            
        }
        public string UniqCode { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string ReferencesTypeId { get; set; }
        public virtual ReferencesType ReferencesType  { get; set; }
    }
}
